﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Transforms.Image;
using Microsoft.ML;

namespace Backend
{
    public partial class MapDetection
    {
        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.LoadImages(outputColumnName:@"input",imageFolder:@"",inputColumnName:@"ImageSource")      
                                    .Append(mlContext.Transforms.ResizeImages(imageWidth:800,imageHeight:600,outputColumnName:@"input",inputColumnName:@"input",cropAnchor:ImageResizingEstimator.Anchor.Center,resizing:ImageResizingEstimator.ResizingKind.Fill))      
                                    .Append(mlContext.Transforms.ExtractPixels(outputColumnName:@"input",inputColumnName:@"input",colorsToExtract:ImagePixelExtractingEstimator.ColorBits.Rgb,orderOfExtraction:ImagePixelExtractingEstimator.ColorsOrder.ARGB))      
                                    .Append(mlContext.Transforms.ApplyOnnxModel(modelFile:@"C:\Users\lgszu\source\repos\MapsVisualisation\Backend\MapDetection.onnx"));

            return pipeline;
        }
    }
}
