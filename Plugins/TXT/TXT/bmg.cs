﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PluginInterface;

namespace TXT
{
    public class bmg
    {
        IPluginHost iPluginHost;
        string archivo;

        public bmg(IPluginHost iPluginHost, string file)
        {
            this.iPluginHost = iPluginHost;
            this.archivo = file;
        }

        public System.Windows.Forms.Control ShowInfo()
        {
            Console.WriteLine("Detectado archivo de texto <b>MESGbmg1</b>. Analizando...");
            sBMG bmg = new sBMG();
            BinaryReader br = new BinaryReader(new FileStream(archivo, FileMode.Open));
            
            br.BaseStream.Position = 0x10;
            if (br.ReadUInt16() != 0x02)
            {
                br.Close();
                return ShowInfo_BigEndian();
            }
            br.BaseStream.Position = 0x00;

            // Cabecera
            bmg.cabecera.id = br.ReadChars(8);
            bmg.cabecera.file_size = br.ReadUInt32();
            bmg.cabecera.nSection = (ushort)br.ReadUInt32();
            bmg.uft16 = br.ReadUInt32() == 0x02 ? true : false;
            // Sección INF1
            br.BaseStream.Position = 0x20;
            bmg.inf1.magicID = br.ReadChars(4);
            bmg.inf1.length = br.ReadUInt32();
            bmg.inf1.nMsg = br.ReadUInt16();
            Console.WriteLine("Contiene {0} mensajes", bmg.inf1.nMsg.ToString());
            bmg.inf1.offsetLength = br.ReadUInt16();
            bmg.inf1.unknown1 = br.ReadUInt16();
            bmg.inf1.unknown2 = br.ReadUInt16();
            Console.WriteLine("Datos desconocidos {0}, {1}", bmg.inf1.unknown1, bmg.inf1.unknown2);

            bmg.inf1.offset = new uint[bmg.inf1.nMsg];
            if (bmg.inf1.offsetLength == 0x08)
                bmg.inf1.msgType = new uint[bmg.inf1.nMsg];
            for (int i = 0; i < bmg.inf1.nMsg; i++)
            {
                bmg.inf1.offset[i] = br.ReadUInt32();
                if (bmg.inf1.offsetLength == 0x08)
                    bmg.inf1.msgType[i] = br.ReadUInt32();
            }
            // Sección DAT1
            br.BaseStream.Position = 0x20 + bmg.inf1.length;
            bmg.dat1.magicID = br.ReadChars(4);
            bmg.dat1.length = br.ReadUInt32();
            long posIni = 0x20 + bmg.inf1.length + 0x08;

            bmg.dat1.msgs = new string[bmg.inf1.nMsg];
            for (int i = 0; i < bmg.inf1.nMsg; i++)
            {
                br.BaseStream.Position = posIni + bmg.inf1.offset[i];
               

                char c = new char();
                do
                {
                    if (bmg.uft16)
                        c = System.Text.Encoding.Unicode.GetChars(br.ReadBytes(2))[0];
                    else
                    {
                        c = (char)br.ReadByte();
                        if (c >= '\x80')
                        {
                            br.BaseStream.Position -= 1;
                            c = System.Text.Encoding.UTF8.GetChars(br.ReadBytes(2))[0];
                        }
                    }

                    if (c == '\x1A') // TODO: Información extra aún no investigada
                    {
                        br.ReadBytes(br.ReadByte() - 0x2);
                        continue;
                    }
                    if (c == '\x0A')
                    {
                        bmg.dat1.msgs[i] += '\r';
                        c = '\n';
                    }

                    bmg.dat1.msgs[i] += c;
                }
                while (c != 0x00);
            }

            br.Close();
            return new iBMG(iPluginHost, bmg);
        }
        public System.Windows.Forms.Control ShowInfo_BigEndian()
        {
            sBMG bmg = new sBMG();
            BinaryReader br = new BinaryReader(new FileStream(archivo, FileMode.Open));

            // Cabecera
            bmg.cabecera.id = br.ReadChars(8);
            bmg.cabecera.file_size = BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            bmg.cabecera.nSection = (ushort)BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            bmg.uft16 = br.ReadUInt32() == 0x02 ? true : false;
            // Sección INF1
            br.BaseStream.Position = 0x20;
            bmg.inf1.magicID = br.ReadChars(4);
            bmg.inf1.length = BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0); ;
            bmg.inf1.nMsg = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            Console.WriteLine("Contiene {0} mensajes", bmg.inf1.nMsg.ToString());
            bmg.inf1.offsetLength = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            bmg.inf1.unknown1 = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            bmg.inf1.unknown2 = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
            Console.WriteLine("Datos desconocidos {0}, {1}", bmg.inf1.unknown1, bmg.inf1.unknown2);

            bmg.inf1.offset = new uint[bmg.inf1.nMsg];
            if (bmg.inf1.offsetLength == 0x08)
                bmg.inf1.msgType = new uint[bmg.inf1.nMsg];
            for (int i = 0; i < bmg.inf1.nMsg; i++)
            {
                bmg.inf1.offset[i] = BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
                if (bmg.inf1.offsetLength == 0x08)
                    bmg.inf1.msgType[i] = BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            }
            // Sección DAT1
            br.BaseStream.Position = 0x20 + bmg.inf1.length;
            bmg.dat1.magicID = br.ReadChars(4);
            bmg.dat1.length = BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            long posIni = 0x20 + bmg.inf1.length + 0x08;

            bmg.dat1.msgs = new string[bmg.inf1.nMsg];
            for (int i = 0; i < bmg.inf1.nMsg; i++)
            {
                br.BaseStream.Position = posIni + bmg.inf1.offset[i];

                char c = new char();
                do
                {
                    if (bmg.uft16)
                        c = System.Text.Encoding.Unicode.GetChars(br.ReadBytes(2))[0];
                    else
                    {
                        c = (char)br.ReadByte();
                        if (c >= '\x80')
                        {
                            br.BaseStream.Position -= 1;
                            c = System.Text.Encoding.UTF8.GetChars(br.ReadBytes(2))[0];
                        }
                    }

                    if (c == '\x1A') // TODO: Información extra aún no investigada
                    {
                        br.ReadBytes(br.ReadByte() - 0x2);
                        continue;
                    }
                    if (c == '\x0A')
                    {
                        bmg.dat1.msgs[i] += '\r';
                        c = '\n';
                    }

                    bmg.dat1.msgs[i] += c;
                }
                while (c != 0x00);
            }

            br.Close();
            return new iBMG(iPluginHost, bmg);
        }
    }

    public struct sBMG
    {
        public Header cabecera;
        public bool uft16;
        public sINF1 inf1;
        public sDAT1 dat1;
    }
    public struct sINF1
    {
        public char[] magicID;
        public uint length;
        public ushort nMsg;
        public ushort offsetLength;
        public ushort unknown1;
        public ushort unknown2;
        public uint[] offset;
        public uint[] msgType;
    }
    public struct sDAT1
    {
        public char[] magicID;
        public uint length;
        public string[] msgs;
    }
}
