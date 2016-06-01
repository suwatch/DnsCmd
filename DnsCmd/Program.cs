using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AntaresAzureDNS;

namespace DnsCmd
{
    class Program
    {
        // Thumbprint: 83CBABE01E09357A428ED22B8A5A9C88D3FDA323
        // PFX: \\iisdist\PublicLockBox\Antares\AntaresInt_02.pfx

        const int TTLInSeconds = 60;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usages: ");
                    Console.WriteLine("    DnsCmd.exe ListARecords host.functionsuw200.kudu1.antares-test.windows-int.net");
                    Console.WriteLine("    DnsCmd.exe AddARecord host.functionsuw200.kudu1.antares-test.windows-int.net 23.96.248.52");
                    Console.WriteLine("    DnsCmd.exe AddARecord host.functionsuw200.kudu1.antares-test.windows-int.net 52.160.111.161");
                    Console.WriteLine("    DnsCmd.exe DeleteARecord host.functionsuw200.kudu1.antares-test.windows-int.net 23.96.248.52");
                    Console.WriteLine("    DnsCmd.exe DeleteARecord host.functionsuw200.kudu1.antares-test.windows-int.net 52.160.111.161");
                    Console.WriteLine("    DnsCmd.exe DeleteAllARecords host.functionsuw200.kudu1.antares-test.windows-int.net");
                    Console.WriteLine("    DnsCmd.exe ListCNames functionsuw200.kudu1.antares-test.windows-int.net");
                    Console.WriteLine("    DnsCmd.exe AddCName functionsuw200.kudu1.antares-test.windows-int.net host.functionsuw200.kudu1.antares-test.windows-int.net");
                    Console.WriteLine("    DnsCmd.exe DeleteCName functionsuw200.kudu1.antares-test.windows-int.net");
                    return;
                }

                switch (args[0])
                {
                    case "ListCNames":
                        ListCNames(args[1]);
                        break;
                    case "AddCName":
                        AddCName(args[1], args[2]);
                        break;
                    case "DeleteCName":
                        DeleteCName(args[1]);
                        break;
                    case "ListARecords":
                        ListARecords(args[1]);
                        break;
                    case "AddARecord":
                        AddARecord(args[1], args[2]);
                        break;
                    case "DeleteARecord":
                        DeleteARecord(args[1], args[2]);
                        break;
                    case "DeleteAllARecords":
                        DeleteAllARecords(args[1]);
                        break;
                    default:
                        throw new ArgumentException(String.Format("{0} not supported", args[0]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void ListCNames(string name)
        {
            var cnames = DNSHelper.ListAllCNameRecords(name);
            Console.WriteLine("List of CNames for {0}", name);
            for (int i = 0; i < cnames.Length; ++i)
            {
                Console.WriteLine("{0}. {1}", i + 1, cnames[i]);
            }
            Console.WriteLine("Total = {0}", cnames.Length);
        }

        static void AddCName(string name, string cname)
        {
            var id = DNSHelper.RegisterCName(hostName: name,
                          adminZone: false,
                          targetName: cname,
                          allowToUpdate: true,
                          ttl: TTLInSeconds);
            Console.WriteLine("Successful with {0}", id);
        }

        static void DeleteCName(string name)
        {
            DNSHelper.DeleteCName(hostName: name);
            Console.WriteLine("Successful");
        }

        static void ListARecords(string name)
        {
            var addresses = DNSHelper.ListAllARecords(name);
            Console.WriteLine("List of ARecords for {0}", name);
            for (int i = 0; i < addresses.Length; ++i)
            {
                Console.WriteLine("{0}. {1}", i + 1, addresses[i]);
            }
            Console.WriteLine("Total = {0}", addresses.Length);
        }

        static void AddARecord(string name, string address)
        {
            var id = DNSHelper.CreateARecord(name, address, useAdminDnsZone: false, ttlInSeconds: TTLInSeconds);
            Console.WriteLine("Successful with {0}", id);
        }

        static void DeleteARecord(string name, string address)
        {
            DNSHelper.DeleteARecord(name, address, useAdminDnsZone: false);
            Console.WriteLine("Successful");
        }

        static void DeleteAllARecords(string name)
        {
            var addresses = DNSHelper.ListAllARecords(name);
            Console.WriteLine("List of ARecords for {0}", name);
            for (int i = 0; i < addresses.Length; ++i)
            {
                DNSHelper.DeleteARecord(name, addresses[i], useAdminDnsZone: false);
                Console.WriteLine("{0}. {1} deleted", i + 1, addresses[i]);
            }
            Console.WriteLine("Successful");
        }
    }
}
