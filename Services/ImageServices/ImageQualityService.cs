//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;
//using FellowOakDicom;
//using FellowOakDicom.Imaging;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using Services.ImageServices.Dtos;

//namespace Services.ImageServices
//{
//    public class ImageQualityService : IImageQualityService
//    {
//        static ImageQualityService()
//        {
//            // Register ImageSharp as the default image manager for fo-dicom.
//            FellowOakDicom.Imaging.ImageManager.SetImplementation(FellowOakDicom.Imaging.ImageSharpImageManager.Instance);
//        }

//        public async Task<ImageQualityResultDto> ValidateImageAsync(Stream stream, string fileExtension, CancellationToken cancellationToken = default)
//        {
//            var result = new ImageQualityResultDto();
//            Image<Rgb24>? image = null;

//            try
//            {
//                // Reset stream position just in case
//                if (stream.CanSeek)
//                {
//                    stream.Position = 0;
//                }

//                string ext = fileExtension.ToLowerInvariant();

//                if (ext == ".dcm" || ext == ".dicom")
//                {
//                    // Process DICOM using fo-dicom
//                    var dicomFile = await DicomFile.OpenAsync(stream);
//                    var dicomImage = new DicomImage(dicomFile.Dataset);
//                    var rendered = dicomImage.RenderImage(0); // Render first frame
                    
//                    // Convert to SixLabors ImageSharp image
//                    image = rendered.AsSharpImage().CloneAs<Rgb24>();
//                }
//                else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
//                {
//                    // Process standard image using ImageSharp
//                    image = await Image.LoadAsync<Rgb24>(stream, cancellationToken);
//                }
//                else
//                {
//                    result.IsValid = false;
//                    result.Errors.Add($"Unsupported file type: '{ext}'. Allowed types are JPG, JPEG, PNG, DICOM.");
//                    return result;
//                }

//                if (image == null)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Failed to decode image.");
//                    return result;
//                }

//                int width = image.Width;
//                int height = image.Height;

//                // 1. Resolution Check
//                if (width < 1024 || height < 1024)
//                {
//                    result.Errors.Add($"Low resolution: {width}x{height}. Minimum required resolution is 1024x1024.");
//                }

//                // Compute luminance buffer for analysis
//                double[,] luminance = new double[width, height];
//                double sumLuminance = 0;

//                image.ProcessPixelRows(accessor =>
//                {
//                    for (int y = 0; y < accessor.Height; y++)
//                    {
//                        var pixelRow = accessor.GetRowSpan(y);
//                        for (int x = 0; x < pixelRow.Length; x++)
//                        {
//                            var pixel = pixelRow[x];
//                            // Compute luminance Y = 0.299*R + 0.587*G + 0.114*B
//                            double yVal = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
//                            luminance[x, y] = yVal;
//                            sumLuminance += yVal;
//                        }
//                    }
//                });

//                double meanBrightness = sumLuminance / (width * height);
//                result.Brightness = meanBrightness;

//                // 2. Brightness Check
//                if (meanBrightness < 40)
//                {
//                    result.Errors.Add($"Image is too dark. Brightness score: {meanBrightness:F1} (Minimum required: 40).");
//                }
//                else if (meanBrightness > 220)
//                {
//                    result.Errors.Add($"Image is too bright/exposed. Brightness score: {meanBrightness:F1} (Maximum allowed: 220).");
//                }

//                // 3. Contrast Check (Standard Deviation of Luminance)
//                double sumSqDiff = 0;
//                for (int y = 0; y < height; y++)
//                {
//                    for (int x = 0; x < width; x++)
//                    {
//                        double diff = luminance[x, y] - meanBrightness;
//                        sumSqDiff += diff * diff;
//                    }
//                }
//                double standardDeviation = Math.Sqrt(sumSqDiff / (width * height));
//                result.Contrast = standardDeviation;

//                if (standardDeviation < 30)
//                {
//                    result.Errors.Add($"Image contrast is too low. Contrast score: {standardDeviation:F1} (Minimum required: 30).");
//                }

//                // 4. Blur Check (Variance of Laplacian)
//                double[,] kernel = {
//                    { 0,  1, 0 },
//                    { 1, -4, 1 },
//                    { 0,  1, 0 }
//                };

//                double[] laplacianValues = new double[(width - 2) * (height - 2)];
//                int idx = 0;
//                double sumLaplacian = 0;

//                for (int y = 1; y < height - 1; y++)
//                {
//                    for (int x = 1; x < width - 1; x++)
//                    {
//                        double val = 0;
//                        for (int ky = -1; ky <= 1; ky++)
//                        {
//                            for (int kx = -1; kx <= 1; kx++)
//                            {
//                                val += luminance[x + kx, y + ky] * kernel[kx + 1, ky + 1];
//                            }
//                        }
//                        laplacianValues[idx++] = val;
//                        sumLaplacian += val;
//                    }
//                }

//                double meanLaplacian = sumLaplacian / laplacianValues.Length;
//                double sumSqDiffLaplacian = 0;
//                foreach (var val in laplacianValues)
//                {
//                    double diff = val - meanLaplacian;
//                    sumSqDiffLaplacian += diff * diff;
//                }

//                double blurVariance = sumSqDiffLaplacian / laplacianValues.Length;
//                result.BlurScore = blurVariance;

//                // If blurVariance is low, the image lacks edge detail (blurry)
//                if (blurVariance < 100.0)
//                {
//                    result.Errors.Add($"Image is too blurry. Blur score: {blurVariance:F1} (Minimum required: 100.0).");
//                }

//                result.IsValid = result.Errors.Count == 0;
//            }
//            catch (Exception ex)
//            {
//                result.IsValid = false;
//                result.Errors.Add($"An error occurred during image quality assessment: {ex.Message}");
//            }
//            finally
//            {
//                image?.Dispose();
//            }

//            return result;
//        }
//    }
//}
