﻿using System;
using UnityEngine;
using VladislavTsurikov.ComponentStack.Scripts;
using VladislavTsurikov.ComponentStack.ScriptsEditor;
using VladislavTsurikov.MegaWorld.CommonScripts.Scripts.CPUNoise;
using VladislavTsurikov.MegaWorld.CommonScripts.Scripts.Other;
using VladislavTsurikov.MegaWorld.Core.Scripts.SettingsSystem;

namespace VladislavTsurikov.MegaWorld.CommonScripts.Scripts.Settings
{
    [Serializable]
    [Name("Simple Filter Settings")]
    public class SimpleFilterComponent : BaseComponent
    {
        [SerializeField]
        public bool UseFalloff = true;

        public bool CheckHeight = false;
        public bool CheckSlope = false; 
        public bool CheckGlobalFractalNoise = false;
        
        #region Height Variables
        public float MinHeight = 0f;
        public float MaxHeight = 0f;

        public FalloffType HeightFalloffType = FalloffType.Add;
        public bool HeightFalloffMinMax = false;

        public float MinAddHeightFalloff = 20;
        public float MaxAddHeightFalloff = 20;

        [Min(0)]
        public float AddHeightFalloff = 20;

        #endregion

        #region Slope Variables

        [Range (0, 90)]
        public float MinSlope = 0f;

        [Range (0, 90)]
        public float MaxSlope = 20f;

        public FalloffType SlopeFalloffType = FalloffType.Add;
        public bool SlopeFalloffMinMax = true;

        public float MinAddSlopeFalloff = 30;
        public float MaxAddSlopeFalloff = 30;

        [Min(0)]
        public float AddSlopeFalloff = 30;
        #endregion

        #region Fractal Noise Settings
        public Texture2D NoiseTexture;
        public FractalNoiseCPU Fractal = new FractalNoiseCPU(new PerlinNoiseCPU(0, 20), 3, 0.03f, 2, 0.5f);

        public float RangeMin = -0.5f;
        public float RangeMax = 0.5f;
        public bool NoisePreviewTexture = false;

        [Range (0, 1)]
        public float RemapNoiseMin = 0f;

        [Range (0, 1)]
        public float RemapNoiseMax = 1f;
        public bool Invert = false;
        #endregion

        public float GetFitness(Vector3 point, Vector3 normal)
        {
            return GetFitness(new Vector2(point.x, point.z), point.y, Vector3.Angle(Vector3.up, normal));
        }

        public float GetFitness(Vector2 point, float height, float slope)
        {
            float fitnessHeight = GetFitnessHeight(height, point);
            float fitnessSlope = GetFitnessSlope(slope);
            float fitnessNoise = GetFitnessGlobalFractalNoise(point);

            return fitnessHeight * fitnessSlope * fitnessNoise;
        }

        public float GetFitnessHeight(float height, Vector2 point)
        {
            return CheckHeight ? GetHeight(height, MinHeight, MaxHeight, point) : 1;
        }

        public float GetFitnessSlope(float slope)
        {
            return CheckSlope ? GetFalloffSlope(slope, MinSlope, MaxSlope) : 1;
        }

        public float GetFitnessGlobalFractalNoise(Vector2 point)
        {
            float fractalValue = 1;
            
            if (CheckGlobalFractalNoise == true && Fractal != null)
            {
                fractalValue = Mathf.InverseLerp(RangeMin, RangeMax, Fractal.Sample2D(point.x, point.y));

                if (Invert == true)
                {
                    fractalValue = 1 - fractalValue;
                }

                if (fractalValue < RemapNoiseMin) 
                {
                    return 0;
                }
                else if(fractalValue > RemapNoiseMax)
                {
                    return 1;
                }
                else
				{
					fractalValue = Mathf.InverseLerp(RemapNoiseMin, RemapNoiseMax, fractalValue);
				}
            }

            return fractalValue;
        }

        public float GetHeight(float height, float minHeight, float maxHeight, Vector2 point)
        {
            switch (HeightFalloffType)
            {
                case FalloffType.Add:
                {
                    float localMinAddHeightFalloff = AddHeightFalloff;
                    float localMaxAddHeightFalloff = AddHeightFalloff;

                    if(HeightFalloffMinMax == true)
                    {
                        localMinAddHeightFalloff = MinAddHeightFalloff;
                        localMaxAddHeightFalloff = MaxAddHeightFalloff;
                    }

                    if(height > maxHeight)
                    {
                        float newMaxHeight = maxHeight + localMaxAddHeightFalloff;

                        return Mathf.InverseLerp(newMaxHeight, maxHeight, height);
                    }
                    else if(height < minHeight)
                    {
                        float newMinHeight = minHeight - localMinAddHeightFalloff;

                        return Mathf.InverseLerp(newMinHeight, minHeight, height);
                    }
                    else
                    {
                        return 1;
                    }
                }
                default:
                {
                    if (height >= minHeight && height <= maxHeight)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public float GetFalloffSlope(float slope, float minSlope, float maxSlope)
        {
            switch (SlopeFalloffType)
            {
                case FalloffType.Add:
                {
                    float localMinAddSlopeFalloff = AddSlopeFalloff;
                    float localMaxAddSlopeFalloff = AddSlopeFalloff;

                    if(SlopeFalloffMinMax == true)
                    {
                        localMinAddSlopeFalloff = MinAddSlopeFalloff;
                        localMaxAddSlopeFalloff = MaxAddSlopeFalloff;
                    }

                    if(slope > maxSlope)
                    {
                        float newMaxSlope = maxSlope + localMaxAddSlopeFalloff;

                        return Mathf.InverseLerp(newMaxSlope, maxSlope, slope);
                    }
                    else if(slope < minSlope)
                    {
                        float newMinSlope = minSlope - localMinAddSlopeFalloff;

                        return Mathf.InverseLerp(newMinSlope, minSlope, slope);
                    }
                    else
                    {
                        return 1;
                    }
                }
                default:
                {
                    if (slope >= minSlope && slope <= maxSlope)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public bool HasOneActiveFilter()
        {
            if(CheckHeight || CheckSlope || CheckGlobalFractalNoise)
            {
                return true;
            }

            return false;
        }
    }
}

