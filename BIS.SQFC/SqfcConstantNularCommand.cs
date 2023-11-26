using System;
using System.Collections.Generic;
using BIS.Core.Streams;
using BIS.SQFC.SqfAst;

namespace BIS.SQFC
{
    internal class SqfcConstantNularCommand : SqfcConstant
    {
        public static HashSet<string> ValidValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Valid constant nular according to
            // https://github.com/dedmen/ArmaScriptCompiler/blob/18a158d2fa2379d954401382e6a3feeccfa5b529/src/optimizer/optimizerModuleConstantFold.cpp#L315C1-L335C41
            "nil",
            "uinamespace",
            "objnull",
            "controlnull",
            "displaynull",
            "grpnull",
            "locationnull",
            "scriptnull",
            "confignull",
            "linebreak",
            "configfile"
        };

        public SqfcConstantNularCommand(string value)
        {
            Value = value;
        }

        public override ConstantType ConstantType => ConstantType.NularCommand;

        public string Value { get; }

        internal override void WriteDataTo(BinaryWriterEx writer, SqfcFile context)
        {
            writer.WriteSqfcString(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        internal override SqfExpression ToExpression(SqfcFile context)
        {
            return new SqfNular(SqfLocation.None, Value);
        }

        public override bool Equals(SqfcConstant other)
        {
            return other is SqfcConstantNularCommand nular && nular.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}