These files, used in many games, give extra information about how the image must be composed.
They are recognized by their header (NSCR or RCSN) and frequently have the same extension, which name could mean _Nitro SCreen Resource_.
They have the format **Map**.

# File structure #

|_Offset_|_Name_|_Type_|_Description_|
|:-------|:-----|:-----|:------------|

**Generic header**

|0x00|ID|char`[4]`|NSCR o RCSN|
|:---|:-|:--------|:----------|
|0x04|Endianess|ushort|If = 0xFFFE the Magic ID must be reversed|
|0x06|Constant|ushort|Unknown|
|0x08|File size|uint|  |
|0x0C|Header size|ushort|Always 0x10|
|0x0E|Number of sections|ushort|Always 0x01|

**NRCS section**

|0x00|ID|char`[4]`|NRCS|
|:---|:-|:--------|:---|
|0x04|Section size|uint|  |
|0x08|Width|ushort|Image width in pixels|
|0x0A|Height|ushort|Image height in pixels|
|0x0C|Unknown|uint|It can be 0x00, 0x01 and 0x02|
|0x10|Data length|uint|Data length (NTFS)|
|0x14|NTFS data|NTFS`[]`|The array length is: Data length / 2|

**NTFS (Nintendo Tile Format Screen) data**

Each structure is 16bits, that is, 2 bytes, and it contains the informations from one tile. The first structure
contains the information for the first tile, the second structure for the second tile and soon...
The structure has the following format (each char is a bit):

```
PPPP X Y NNNNNNNNNN

PPPP => Palette number (in the case of 4bpp palettes where there are more than one palette)
X => Boolean value to flip in the X axis
Y => Boolean value to flip in the Y axis
NNNNNNNNNN => Tile number modified by the above values
```