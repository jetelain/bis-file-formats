using BIS.Core.Streams;

namespace BIS.SQFC.Test
{
    public class SqfcFileTest
    {
        [Fact]
        public void Read()
        {
            var source = typeof(SqfcFileTest).Assembly.GetManifestResourceStream("BIS.SQFC.Test.Data.fnc_strLen.sqfc");

            var file = StreamHelper.Read<SqfcFile>(source);

            Assert.Equal("{\r\n  endStatement;\r\n  get _this;\r\n  push 0;\r\n  binary select;\r\n  unary count;\r\n}".ReplaceLineEndings(), file.ToString().ReplaceLineEndings());
        }
    }
}