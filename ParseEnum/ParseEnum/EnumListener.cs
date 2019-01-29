using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseEnum
{
    public class EnumListener : EnumBaseListener
    {
        EnumParser parser;
        public StringBuilder builder;
        public string name;

        public EnumListener(EnumParser _parser)
        {
            parser = _parser;
            builder = new StringBuilder();
        }

        public override void EnterEnumDeclaration(EnumParser.EnumDeclarationContext context)
        {
            name = Char.ToLowerInvariant(context.ID().ToString()[0]) + context.ID().ToString().Substring(1);
            builder.AppendLine("const " + name + " {");
        }

        public override void ExitEnumDeclaration(EnumParser.EnumDeclarationContext context)
        {
            builder.AppendLine("};\n");
            builder.AppendLine("export default " + name + ";");
        }

        public override void EnterAssign(EnumParser.AssignContext context)
        {
            ITokenStream tokens = (ITokenStream)parser.InputStream;
            String args = tokens.GetText(context.expr());
            builder.AppendLine("\t" + context.ID() + ": " + args + ",");
        }
    }
}
