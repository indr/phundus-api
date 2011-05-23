using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using phiNdus.fundus.Core.Business.IntegrationTests.TestHelpers;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Helpers
{
    public class Pop3 : TcpClient
    {
        public void Connect(string server, string username, string password)
        {
            string message;
            string response;

            Connect(server, 110);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }

            message = "USER " + username + "\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }

            message = "PASS " + password + "\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }
        }

        public void Disconnect()
        {
            string message;
            string response;
            message = "QUIT\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }
        }

        public ICollection<Pop3Message> List()
        {
            string message;
            string response;

            var retval = new List<Pop3Message>();
            message = "LIST\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }

            while (true)
            {
                response = Response();
                if (response == ".\r\n")
                {
                    return retval;
                }
                else
                {
                    var msg = new Pop3Message();
                    char[] seps = {' '};
                    var values = response.Split(seps);
                    msg.number = Int32.Parse(values[0]);
                    msg.bytes = Int32.Parse(values[1]);
                    msg.retrieved = false;
                    retval.Add(msg);
                    continue;
                }
            }
        }

        public Pop3Message Retrieve(Pop3Message rhs)
        {
            string message;
            string response;

            var msg = new Pop3Message();
            msg.bytes = rhs.bytes;
            msg.number = rhs.number;

            message = "RETR " + rhs.number + "\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }

            msg.retrieved = true;
            while (true)
            {
                response = Response();
                if (response == ".\r\n")
                {
                    break;
                }
                else
                {
                    msg.message += response;
                }
            }

            return msg;
        }

        public void Delete(Pop3Message rhs)
        {
            string message;
            string response;

            message = "DELE " + rhs.number + "\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }
        }

        private void Write(string message)
        {
            var en = new ASCIIEncoding();

            var WriteBuffer = new byte[1024];
            WriteBuffer = en.GetBytes(message);

            var stream = GetStream();
            stream.Write(WriteBuffer, 0, WriteBuffer.Length);

            //Debug.WriteLine("WRITE:" + message);
        }

        private string Response()
        {
            var enc = new ASCIIEncoding();
            var serverbuff = new Byte[1024];
            var stream = GetStream();
            var count = 0;
            while (true)
            {
                var buff = new Byte[2];
                var bytes = stream.Read(buff, 0, 1);
                if (bytes == 1)
                {
                    serverbuff[count] = buff[0];
                    count++;

                    if (buff[0] == '\n')
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                ;
            }
            ;

            var retval = enc.GetString(serverbuff, 0, count);
            //Debug.WriteLine("READ:" + retval);
            return retval;
        }

        public void DeleteAll()
        {
            foreach (var raw in List())
                Delete(raw);
        }

        public Pop3Message Find(string subject)
        {
            foreach (var raw in List())
            {
                var msg = Retrieve(raw);
                if (msg.message.Contains("Subject: " + subject))
                    return msg;
            }
            return null;
        }


        public static Pop3Message RetrieveMail(string host, string username, string password, string subject)
        {
            var pop = new Pop3();
            pop.Connect(host, username, password);
            var result = pop.Find(subject);
            pop.DeleteAll();
            pop.Disconnect();
            return result;
        }
    }
}