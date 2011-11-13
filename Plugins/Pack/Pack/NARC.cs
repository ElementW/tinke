﻿/*
 * Copyright (C) 2011  pleoNeX
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 *
 * Programador: pleoNeX
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using PluginInterface;

namespace Pack
{
    public class NARC 
    {
        IPluginHost pluginHost;
        string narcFile;
        ARC narc;

        public NARC(IPluginHost pluginHost)
        {
            this.pluginHost = pluginHost;
        }

        #region Unpack methods
        public sFolder Unpack(string archivo)
        {
            ARC arc = new ARC();
            narcFile = pluginHost.Get_TempFolder() + Path.DirectorySeparatorChar + "pack_" + new FileInfo(archivo).Name;
            File.Copy(archivo, narcFile, true);
            BinaryReader br = new BinaryReader(System.IO.File.OpenRead(archivo));

            // Lee cabecera genérica e inicial:
            arc.id = br.ReadChars(4);
            arc.id_endian = br.ReadUInt16();
            if (arc.id_endian == 0xFFFE)
                arc.id.Reverse<Char>();
            arc.constant = br.ReadUInt16();
            arc.file_size = br.ReadUInt32();
            arc.header_size = br.ReadUInt16();
            arc.nSections = br.ReadUInt16();

            // Lee primera sección BTAF (File Allocation TaBle)
            arc.btaf.id = br.ReadChars(4);
            arc.btaf.section_size = br.ReadUInt32();
            arc.btaf.nFiles = br.ReadUInt32();
            arc.btaf.entries = new BTAF_Entry[arc.btaf.nFiles];
            for (int i = 0; i < arc.btaf.nFiles; i++)
            {
                arc.btaf.entries[i].start_offset = br.ReadUInt32();
                arc.btaf.entries[i].end_offset = br.ReadUInt32();
            }

            // Lee la segunda sección BTNF (File Name TaBle)
            arc.btnf.id = br.ReadChars(4);
            arc.btnf.section_size = br.ReadUInt32();
            arc.btnf.entries = new List<BTNF_MainEntry>();
            long pos = br.BaseStream.Position;
            #region Obtener carpeta root
            do
            {
                BTNF_MainEntry main = new BTNF_MainEntry();
                main.offset = br.ReadUInt32();
                main.first_pos = br.ReadUInt16();
                main.parent = br.ReadUInt16();
                uint idFile = main.first_pos;

                if (main.offset < 0x8)  // No hay nombres, juegos como el pokemon
                {

                    for (int i = 0; i < arc.btaf.nFiles; i++)
                    {
                        sFile currFile = new sFile();
                        currFile.id = (ushort)idFile; idFile++;
                        currFile.name = "file" + idFile.ToString();
                        if (!(main.files is List<sFile>))
                            main.files = new List<sFile>();
                        main.files.Add(currFile);
                    }
                    br.BaseStream.Position = main.offset + pos; // Para que funcione la condición while
                    arc.btnf.entries.Add(main);
                    continue;
                }
                long posmain = br.BaseStream.Position;
                br.BaseStream.Position = main.offset + pos;
                int id = br.ReadByte();

                while (id != 0x0)   // Indicador de fin de subtable
                {
                    if (id < 0x80)  // Es archivo
                    {
                        sFile currFile = new sFile();
                        currFile.id = (ushort)idFile;
                        idFile++;
                        currFile.name = new String(br.ReadChars(id));
                        if (!(main.files is List<sFile>))
                            main.files = new List<sFile>();
                        main.files.Add(currFile);
                    }
                    else if (id > 0x80) // Es carpeta
                    {
                        sFolder currFolder = new sFolder();
                        currFolder.name = new String(br.ReadChars(id - 0x80));
                        currFolder.id = br.ReadUInt16();
                        if (!(main.folders is List<sFolder>))
                            main.folders = new List<sFolder>();
                        main.folders.Add(currFolder);
                    }

                    id = br.ReadByte();
                }
                arc.btnf.entries.Add(main);
                br.BaseStream.Position = posmain;

            } while (arc.btnf.entries[0].offset + pos != br.BaseStream.Position);

            br.BaseStream.Position = pos - 8 + arc.btnf.section_size;
            sFolder root = Jerarquizar_Carpetas(arc.btnf.entries, 0xF000, "root");
            #endregion

            // Lee tercera sección GMIF (File IMaGe)
            arc.gmif.id = br.ReadChars(4);
            arc.gmif.section_size = br.ReadUInt32();
            pos = br.BaseStream.Position;
            for (int i = 0; i < arc.btaf.nFiles; i++)
            {
                Asignar_Archivos(root, i,
                    arc.btaf.entries[i].end_offset - arc.btaf.entries[i].start_offset,
                    (uint)pos + arc.btaf.entries[i].start_offset);
            }

            br.Close();
            narc = arc;
            return root;
        }
        private sFolder Jerarquizar_Carpetas(List<BTNF_MainEntry> entries, int idFolder, string nameFolder)
        {
            sFolder currFolder = new sFolder();

            currFolder.name = nameFolder;
            currFolder.id = (ushort)idFolder;
            currFolder.files = entries[idFolder & 0xFFF].files;

            if (entries[idFolder & 0xFFF].folders is List<sFolder>) // Si tiene carpetas dentro.
            {
                currFolder.folders = new List<sFolder>();

                foreach (sFolder subFolder in entries[idFolder & 0xFFF].folders)
                    currFolder.folders.Add(Jerarquizar_Carpetas(entries, subFolder.id, subFolder.name));
            }

            return currFolder;
        }
        private void Asignar_Archivos(sFolder currFolder, int idFile, UInt32 size, UInt32 offset)
        {
            if (currFolder.files is List<sFile>)
            {
                for (int i = 0; i < currFolder.files.Count; i++)
                {
                    if (currFolder.files[i].id == idFile)
                    {
                        sFile newFile = currFolder.files[i];
                        newFile.size = size;
                        newFile.offset = offset;
                        newFile.path = narcFile;
                        currFolder.files.RemoveAt(i);
                        currFolder.files.Insert(i, newFile);
                        return;
                    }

                }
            }

            if (currFolder.folders is List<sFolder>) // Si tiene carpetas dentro.
                foreach (sFolder subFolder in currFolder.folders)
                    Asignar_Archivos(subFolder, idFile, size, offset);
        }
        #endregion

        #region Pack methods
        public String Pack(string file, ref sFolder unpacked)
        {
            Unpack(file);
            String fileout = pluginHost.Get_TempFolder() + Path.DirectorySeparatorChar + "Newpack_" + Path.GetFileName(file);
            if (File.Exists(fileout))
                File.Delete(fileout);

            Save_NARC(fileout, ref unpacked);
            return fileout;
        }
        private void Save_NARC(string fileout, ref sFolder decompressed)
        {
            /* Structure of the file
             * 
             * Common header
             * 
             * BTAF section
             * |_ Start offset
             * |_ End offset
             * 
             * BTNF section
             * 
             * GMIF section
             * |_ Files
             * 
             */

            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileout));
            BinaryReader br = new BinaryReader(File.OpenRead(narc.file));

            // Write the BTAF section
            String btafTmp = Path.GetTempFileName();
            Write_BTAF(
                btafTmp,
                0x10 + narc.btaf.section_size + narc.btnf.section_size + 0x08,
                ref decompressed);

            // Write the BTNF section
            String btnfTmp = Path.GetTempFileName();
            br.BaseStream.Position = 0x10 + narc.btaf.section_size;
            File.WriteAllBytes(btnfTmp, br.ReadBytes((int)narc.btnf.section_size));

            // Write the GMIF section
            String gmifTmp = Path.GetTempFileName();
            Write_GMIF(gmifTmp, decompressed);

            // Write the NARC file
            int file_size = (int)(narc.header_size + narc.btaf.section_size + narc.btnf.section_size +
                narc.gmif.section_size);

            // Common header
            bw.Write(narc.id);
            bw.Write(narc.id_endian);
            bw.Write(narc.constant);
            bw.Write(file_size);
            bw.Write(narc.header_size);
            bw.Write(narc.nSections);
            // Write the sections
            bw.Write(File.ReadAllBytes(btafTmp));
            bw.Write(File.ReadAllBytes(btnfTmp));
            bw.Write(narc.gmif.id);
            bw.Write(narc.gmif.section_size);
            bw.Write(File.ReadAllBytes(gmifTmp));

            bw.Flush();
            bw.Close();
            br.Close();

            File.Delete(btafTmp);
            File.Delete(btnfTmp);
            File.Delete(gmifTmp);
        }
        private void Write_BTAF(string fileout, uint startOffset, ref sFolder decompressed)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileout));
            uint offset = 0;

            bw.Write(narc.btaf.id);
            bw.Write(narc.btaf.section_size);
            bw.Write(narc.btaf.nFiles);

            for (int i = 0; i < narc.btaf.nFiles; i++)
            {
                sFile currFile = Search_File(i + decompressed.id, decompressed);
                currFile.offset = offset;

                bw.Write(offset);
                offset += currFile.size;
                bw.Write(offset);
            }

            bw.Flush();
            bw.Close();
        }
        private void Write_GMIF(string fileout, sFolder decompressed)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileout));

            for (int i = 0; i < narc.btaf.nFiles; i++)
            {
                sFile currFile = Search_File(i + decompressed.id, decompressed);
                BinaryReader br = new BinaryReader(File.OpenRead(currFile.path));
                br.BaseStream.Position = currFile.offset;

                bw.Write(br.ReadBytes((int)currFile.size));
                br.Close();
                bw.Flush();
            }

            bw.Flush();
            bw.Close();
            narc.gmif.section_size = (uint)(new FileInfo(fileout).Length) + 0x08;
        }
        private sFile Search_File(int id, sFolder currFolder)
        {
            if (currFolder.files is List<sFile>)
                foreach (sFile archivo in currFolder.files)
                    if (archivo.id == id)
                        return archivo;


            if (currFolder.folders is List<sFolder>)
            {
                foreach (sFolder subFolder in currFolder.folders)
                {
                    sFile currFile = Search_File(id, subFolder);
                    if (currFile.name is string)
                        return currFile;
                }
            }

            return new sFile();
        }
        #endregion

    }

    #region Estructuras
    public struct ARC
    {
        public int file_id;
        public string file;

        public char[] id;           // Always NARC = 0x4E415243
        public UInt16 id_endian;    // Si 0xFFFE hay que darle la vuelta al id
        public UInt16 constant;     // Always 0x0100
        public UInt32 file_size;
        public UInt16 header_size;  // Siempre 0x0010
        public UInt16 nSections;    // En este caso siempre 0x0003

        public BTAF btaf;
        public BTNF btnf;
        public GMIF gmif;
    }
    public struct BTAF
    {
        public char[] id;
        public UInt32 section_size;
        public UInt32 nFiles;
        public BTAF_Entry[] entries;
    }
    public struct BTAF_Entry
    {
        // Ambas son relativas a la sección GMIF
        public UInt32 start_offset;
        public UInt32 end_offset;
    }
    public struct BTNF
    {
        public char[] id;
        public UInt32 section_size;
        public List<BTNF_MainEntry> entries;
    }
    public struct BTNF_MainEntry
    {
        public UInt32 offset;       // Relativo a la primera entrada
        public UInt32 first_pos;    // ID del primer archivo.
        public UInt32 parent;       // En el caso de root, número de carpetas;
        public List<sFile> files;
        public List<sFolder> folders;
    }
    public struct GMIF
    {
        public char[] id;
        public UInt32 section_size;
        // Datos de los archivos....
    }
    #endregion

}
