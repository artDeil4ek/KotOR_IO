﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace KotOR_IO
{
    public partial class GFF
    {
        public class StrRef : FIELD
        {
            public int leading_value;
            public int reference;

            public StrRef() { }
            public StrRef(string Label, int leading_value, int reference)
            {
                this.Type = 18;
                if (Label.Length > 16) { throw new Exception($"Label \"{Label}\" is longer than 16 characters, and is invalid."); }
                this.Label = Label;
                this.leading_value = leading_value;
                this.reference = reference;
            }
            internal StrRef(BinaryReader br, int offset)
            {
                //Header Info
                br.BaseStream.Seek(24, 0);
                int LabelOffset = br.ReadInt32();
                int LabelCount = br.ReadInt32();
                int FieldDataOffset = br.ReadInt32();

                //Basic Field Data
                br.BaseStream.Seek(offset, 0);
                Type = br.ReadInt32();
                int LabelIndex = br.ReadInt32();
                int DataOrDataOffset = br.ReadInt32();

                //Label Logic
                br.BaseStream.Seek(LabelOffset + LabelIndex * 16, 0);
                Label = new string(br.ReadChars(16)).TrimEnd('\0');

                //Comlex Value Logic
                br.BaseStream.Seek(FieldDataOffset + DataOrDataOffset, 0);
                leading_value = br.ReadInt32();
                reference = br.ReadInt32();

            }

            internal override void collect_fields(ref List<Tuple<FIELD, int, int>> Field_Array, ref List<byte> Raw_Field_Data_Block, ref List<string> Label_Array, ref int Struct_Indexer, ref int List_Indices_Counter)
            {
                Tuple<FIELD, int, int> T = new Tuple<FIELD, int, int>(this, Raw_Field_Data_Block.Count, this.GetHashCode());
                Raw_Field_Data_Block.AddRange(BitConverter.GetBytes(leading_value));
                Raw_Field_Data_Block.AddRange(BitConverter.GetBytes(reference));
                Field_Array.Add(T);

                if (!Label_Array.Contains(Label))
                {
                    Label_Array.Add(Label);
                }
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || !GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    return leading_value == (obj as StrRef).leading_value && reference == (obj as StrRef).reference && Label == (obj as StrRef).Label;
                }
            }

            public override int GetHashCode()
            {
                return new { Type, leading_value, reference, Label }.GetHashCode();
            }


        }
    }
} 