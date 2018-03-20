using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUtilities {

    public static Font GetFont(string _fileName)
    {
        Font font;
        font = Resources.Load<Font>("Fonts/" + _fileName);
        return font;
    }

    public static TMP_FontAsset GetTmpFontAsset(string _fileName)
    {
        TMP_FontAsset tmpFontAsset;
        tmpFontAsset = Resources.Load<TMP_FontAsset>("Fonts/" + _fileName);
        return tmpFontAsset;
    }

    public static string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    }; 
    
}
