using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Isam.Esent.Interop;
using System.IO;

namespace EsentTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            JET_INSTANCE instance;

            Api.JetCreateInstance(out instance, Guid.NewGuid().ToString());

            var backupLocation = Path.GetFullPath("../../Backup/DB1");
            var databaseLocation = Path.GetFullPath("Restore/DB10");

            if(Directory.Exists(databaseLocation)) {
                Directory.Delete(databaseLocation, true);
            }

            Directory.CreateDirectory(databaseLocation);

            Directory.CreateDirectory(Path.Combine(databaseLocation, "logs"));
            Directory.CreateDirectory(Path.Combine(databaseLocation, "temp"));
            Directory.CreateDirectory(Path.Combine(databaseLocation, "system"));
            new Configurator().ConfigureInstance(instance, databaseLocation);

            Api.JetRestoreInstance(instance, backupLocation, databaseLocation, RestoreStatusCallback);

            Console.WriteLine("Done...");
            Console.ReadKey();
        }

        private static JET_err RestoreStatusCallback(JET_SESID sesid, JET_SNP snp, JET_SNT snt, object data)
        {
            Console.WriteLine("Esent Restore: {0} {1} {2}", snp, snt, data);
            return JET_err.Success;
        }
    }
}
