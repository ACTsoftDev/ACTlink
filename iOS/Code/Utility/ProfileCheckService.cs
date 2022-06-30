using System;
using System.Diagnostics;
using Foundation;
using Security;

namespace actchargers.iOS
{
    public class ProfileCheckService : IProfileCheckService
    {
        public bool CheckForProfile(string certificateName)
        {
            try
            {
                if (!string.IsNullOrEmpty(certificateName))
                {
                    string certPath = NSBundle
                        .MainBundle
                        .PathForResource(certificateName, "cer");

                    NSData certData = NSData.FromFile(certPath);
                    SecCertificate cert = new SecCertificate(certData);

                    SecPolicy policy = SecPolicy.CreateBasicX509Policy();
                    SecTrust trust = new SecTrust(cert, policy);

                    SecTrustResult code = trust.Evaluate();

                    if (code == SecTrustResult.Proceed
                        || code == SecTrustResult.Unspecified)
                    {
                        //profile is installed and trusted 
                        Debug.WriteLine("Profile successfully installed and trusted");
                        return true;
                    }
                    else
                    {
                        //error with profile 
                        Debug.WriteLine("Error in profile installation/trust");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
