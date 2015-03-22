These files, which name could be _Nitro CoLour Resource_, are used by many games. They contain the information about
the **palettes** of the image (It can have more than one palette (4-bpp)). They are recognized by their header (NCLR or RLCN)
and frequently have the same extension

# Palette structure #

|_Offset_|_Type_|_Description_|
|:-------|:-----|:------------|

**Generic header**

|0x00|char`[4]`|Magic ID (NCLR)|
|:---|:--------|:--------------|
|0x04|ushort|Endianess (if = 0xFFFE the Magic ID must be reversed)|
|0x06|ushort|Constant|
|0x08|uint|File size|
|0x0C|ushort|Header size|
|0x0E|ushort|# of sections|

**PLTT (PaLeTTe) section**

|0x00|char`[4]`|Magic ID (PLTT)|
|:---|:--------|:--------------|
|0x04|uint|Section size|
|0x08|ushort|Depth (0x03 == 4bpp; 0x04 == 8bpp)|
|0x0A|ushort|Unknown 1|
|0x0C|uint|Unkwnon 2|
|0x10|uint|Palettes size|
|0x14|uint|Start offset of the Color data (from "Section size" 0x14)|

**Palettes data (NTFP Nintendo Tile Format Palette)**

|0x00|byte`[2]`|Color encoded with BGR555|
|:---|:--------|:------------------------|
|...|

**PMCP section (optional)**

|0x00|char`[4]`|Magic ID (PMCP)|
|:---|:--------|:--------------|
|0x04|uint|Block size (should be 0x12)|
|0x08|ushort|Unknown 1|
|0x0A|ushort|Unknown 2|
|0x0C|uint|Unknown 3|
|0x10|ushort|Number of the first palette|

  * _Note:_ The number of palettes can be calculate in this way:

```
int nPalettes = (Palttes size) / (number of colors * 2);
```

  * _Note 1:_ If the depth is 4bpp there are 16 colors, if the depth is 8bpp there are 256 colors

  * _Note 2:_ The value "Number of the first palette" in PMCP section is used with NSCR files,
> there in the map data the first palette to use is this value,
> so instead to be the palette 0 the first palette it can be the 0xE one.

# BGR555 encoding #

Two bytes define a color by three number (BGR Blue Green Red), they are stored in the following form:

XBBBBBGGGGGRRRRR

where each letter is a bit (5 bit per colour), the X letter is never used (always 0).
A way of decoding the two bytes is (in C#):

```
int r, b; double g;
short bgr = BitConverter.ToInt16(new Byte[] { byte1, byte2 }, 0);

r = (bgr & 0x001F) * 0x08;
g = ((bgr & 0x03E0) >> 5) * 0x08;
b = ((bgr & 0x7C00) >> 10) * 0x08;

return System.Drawing.Color.FromArgb(r, (int)g, b);
```
Using 5 bit can be 32 possible colors per attribute (Red, Blue or Green) so we can encode 32768 different colors.