﻿using BIS.Core;
using BIS.Core.Math;
using BIS.Core.Streams;

namespace BIS.P3D.ODOL
{
    public class ModelInfo : IModelInfo
    {
        internal ModelInfo(BinaryReaderEx input, int version, int noOfLods)
        {
            Special = input.ReadInt32();
            BoundingSphere = input.ReadSingle();
            GeometrySphere = input.ReadSingle();
            Remarks = input.ReadInt32();
            AndHints = input.ReadInt32();
            OrHints = input.ReadInt32();
            AimingCenter = new Vector3P(input);
            Color = new PackedColor(input.ReadUInt32());
            ColorType = new PackedColor(input.ReadUInt32());
            ViewDensity = input.ReadSingle();
            BboxMin = new Vector3P(input);
            BboxMax = new Vector3P(input);
            if (version >= 70)
            {
                LodDensityCoef = input.ReadSingle();
            }
            if (version >= 71)
            {
                DrawImportance = input.ReadSingle();
            }
            if (version >= 52)
            {
                BboxMinVisual = new Vector3P(input);
                BboxMaxVisual = new Vector3P(input);
            }
            BoundingCenter = new Vector3P(input);
            GeometryCenter = new Vector3P(input);
            CenterOfMass = new Vector3P(input);
            InvInertia = input.ReadArrayBase(i => new Vector3P(input), 3);
            AutoCenter = input.ReadBoolean();
            LockAutoCenter = input.ReadBoolean();
            CanOcclude = input.ReadBoolean();
            CanBeOccluded = input.ReadBoolean();
            if (version >= 73)
            {
                AICovers = input.ReadBoolean();
            }
            if ((version >= 42 && version < 10000) || version >= 10042)
            {
                HtMin = input.ReadSingle();
                HtMax = input.ReadSingle();
                AfMax = input.ReadSingle();
                MfMax = input.ReadSingle();
            }
            if ((version >= 43 && version < 10000) || version >= 10043)
            {
                MFact = input.ReadSingle();
                TBody = input.ReadSingle();
            }
            if (version >= 33)
            {
                ForceNotAlphaModel = input.ReadBoolean();
            }
            if (version >= 37)
            {
                SbSource = input.ReadInt32();
                Prefershadowvolume = input.ReadBoolean();
            }
            if (version >= 48)
            {
                ShadowOffset = input.ReadSingle();
            }
            Animated = input.ReadBoolean();
            Skeleton = new Skeleton(input, version, noOfLods);
            MapType = input.ReadByte();
            MassArray = input.ReadCompressedFloatArray();
            Mass = input.ReadSingle();
            InvMass = input.ReadSingle();
            Armor = input.ReadSingle();
            InvArmor = input.ReadSingle();
            if (version >= 72)
            {
                ExplosionShielding = input.ReadSingle();
            }
            if (version >= 53)
            {
                GeometrySimple = input.ReadByte();
            }
            if (version >= 54)
            {
                GeometryPhys = input.ReadByte();
            }
            Memory = input.ReadByte();
            Geometry = input.ReadByte();
            GeometryFire = input.ReadByte();
            GeometryView = input.ReadByte();
            GeometryViewPilot = input.ReadByte();
            GeometryViewGunner = input.ReadByte();
            UnknownByte = input.ReadSByte();
            GeometryViewCargo = input.ReadByte();
            LandContact = input.ReadByte();
            Roadway = input.ReadByte();
            Paths = input.ReadByte();
            Hitpoints = input.ReadByte();
            MinShadow = (byte)input.ReadUInt32();
            if (version >= 38)
            {
                CanBlend = input.ReadBoolean();
            }
            Class = input.ReadAsciiz();
            Damage = input.ReadAsciiz();
            Frequent = input.ReadBoolean();
            if (version >= 31)
            {
                input.ReadUInt32();
            }
            if (version >= 57)
            {
                PreferredShadowVolumeLod = input.ReadArrayBase(i => i.ReadInt32(), noOfLods);
                PreferredShadowBufferLod = input.ReadArrayBase(i => i.ReadInt32(), noOfLods);
                PreferredShadowBufferLodVis = input.ReadArrayBase(i => i.ReadInt32(), noOfLods);
            }
        }

        public int Special { get; }
        public float BoundingSphere { get; }
        public float GeometrySphere { get; }
        public int Remarks { get; }
        public int AndHints { get; }
        public int OrHints { get; }
        public Vector3P AimingCenter { get; }
        public PackedColor Color { get; }
        public PackedColor ColorType { get; }
        public float ViewDensity { get; }
        public Vector3P BboxMin { get; }
        public Vector3P BboxMax { get; }
        public float LodDensityCoef { get; }
        public float DrawImportance { get; }
        public Vector3P BboxMinVisual { get; }
        public Vector3P BboxMaxVisual { get; }
        public Vector3P BoundingCenter { get; }
        public Vector3P GeometryCenter { get; }
        public Vector3P CenterOfMass { get; }
        public Vector3P[] InvInertia { get; }
        public bool AutoCenter { get; }
        public bool LockAutoCenter { get; }
        public bool CanOcclude { get; }
        public bool CanBeOccluded { get; }
        public bool AICovers { get; }
        public float HtMin { get; }
        public float HtMax { get; }
        public float AfMax { get; }
        public float MfMax { get; }
        public float MFact { get; }
        public float TBody { get; }
        public bool ForceNotAlphaModel { get; }
        public int SbSource { get; }
        public bool Prefershadowvolume { get; }
        public float ShadowOffset { get; }
        public bool Animated { get; }
        public Skeleton Skeleton { get; }
        public byte MapType { get; }
        public float[] MassArray { get; }
        public float Mass { get; }
        public float InvMass { get; }
        public float Armor { get; }
        public float InvArmor { get; }
        public float ExplosionShielding { get; }
        public byte GeometrySimple { get; }
        public byte GeometryPhys { get; }
        public byte Memory { get; }
        public byte Geometry { get; }
        public byte GeometryFire { get; }
        public byte GeometryView { get; }
        public byte GeometryViewPilot { get; }
        public byte GeometryViewGunner { get; }
        public sbyte UnknownByte { get; }
        public byte GeometryViewCargo { get; }
        public byte LandContact { get; }
        public byte Roadway { get; }
        public byte Paths { get; }
        public byte Hitpoints { get; }
        public byte MinShadow { get; }
        public bool CanBlend { get; }
        public string Class { get; }
        public string Damage { get; }
        public bool Frequent { get; }
        public int[] PreferredShadowVolumeLod { get; }
        public int[] PreferredShadowBufferLod { get; }
        public int[] PreferredShadowBufferLodVis { get; }
    }
}