﻿using VladislavTsurikov.Scripts;

namespace VladislavTsurikov.MegaWorld.CommonScripts.Scripts.Settings.MaskFilters.Noise.API
{
    /// <summary>
    /// Struct defining the description for a NoiseShaderGenerator
    /// </summary>
    public struct ShaderGeneratorDescriptor
    {
        /// <summary>
        /// The name of the NoiseShaderGenerator
        /// </summary>
        public string name;
        /// <summary>
        /// The Shader Category for Shaders generated by the NoiseShaderGenerator
        /// </summary>
        public string shaderCategory;
        /// <summary>
        /// The path specifying where the Shaders generated by the NoiseShaderGenerator
        /// should be written to disk
        /// </summary>
        public string outputDir;
        /// <summary>
        /// The path to the ".noisehlsltemplate" file to be used in the NoiseShaderGenerator's
        /// Shader generation.
        /// </summary>
        public string templatePath;
    }

    /// <summary>
    /// Interface for NoiseShaderGenerator. Do not implement this directly. Inherit from the
    /// NoiseShaderGenerator< T > generic abstract class, instead.
    /// </summary>
    public interface INoiseShaderGenerator
    {
        /// <summary>
        /// Returns a description for the NoiseShaderGenerator implementation
        /// </summary>
        ShaderGeneratorDescriptor GetDescription();
    }

    /// <summary>
    /// Singleton base class for a NoiseShaderGenerator implementation. Inherit from this if you want
    /// your shaders to get generated
    /// </summary>
    public abstract class NoiseShaderGenerator<T> : ScriptableSingleton<T>, INoiseShaderGenerator where T : NoiseShaderGenerator<T>
    {
        /// <summary>
        /// Returns a description for the NoiseShaderGenerator implementation
        /// </summary>
        public abstract ShaderGeneratorDescriptor GetDescription();
    }
}