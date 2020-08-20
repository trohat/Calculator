using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace SOAP_Calculator
{
    class Program
    {
        public HttpWebRequest CreateSOAPWebRequest()
        {
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://www.dneonline.com/calculator.asmx");
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/Add");
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            Req.Method = "POST";
            return Req;
        }

        public void InvokeService(int a, int b)
        {
            //Calling CreateSOAPWebRequest method  
            HttpWebRequest request = CreateSOAPWebRequest();

            /*
            //SOAP Body Request  
            XmlDocument SOAPReqBody = new XmlDocument();

            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                           xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                           xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
             <soap:Body>  
                <Add xmlns=""http://tempuri.org/"">  
                  <intA>" + a + @"</intA>  
                  <intB>" + b + @"</intB>  
                </Add>  
              </soap:Body>  
            </soap:Envelope>");
            */
            
            string soapUrl = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace soap = soapUrl;

            string xsdUrl = "http://www.w3.org/2001/XMLSchema";
            XNamespace xsd = xsdUrl;

            string xsiUrl = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xsi = xsiUrl;

            string tempUriString = "http://tempuri.org/";
            XNamespace tempUri = tempUriString;


            XDocument SOAPReqBody = new XDocument(
                //new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(soap + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", soapUrl),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsdUrl),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsiUrl),
                    new XElement(soap + "Body",
                        new XElement(tempUri + "Add",
                            new XElement(tempUri + "intA", a),
                            new XElement(tempUri + "intB", b)
                        )
                    )
                )
            );

            SOAPReqBody.Save("Root.xml");

            Console.WriteLine(File.ReadAllText("Root.xml"));

            Console.WriteLine("ToString:" + SOAPReqBody.ToString());
            Console.WriteLine("Waiting for resp!");
            //Console.ReadLine();


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request  
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream  
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console  
                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Program obj = new Program();
            Console.WriteLine("Please Enter Input values..");
            //Reading input values from console  
            int a = Convert.ToInt32(Console.ReadLine());
            int b = Convert.ToInt32(Console.ReadLine());
            //Calling InvokeService method  
            obj.InvokeService(a, b);
        }
    }
}
