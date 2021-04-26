using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sql.EFCoreMigration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PostgresEntityConfigAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SqlServerEntityConfigAttribute : Attribute
    {
    }

}
