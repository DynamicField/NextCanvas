﻿namespace NextCanvas.Models.Content
{
    public class TextBoxElement : ContentElement
    {
        public TextBoxElement()
        {
            Width = 425;
            Height = 300;
        }


        public string RtfText { get; set; } /* =
            @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1036{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\*\generator Riched20 10.0.17134}\viewkind4\uc1 
\pard\sa200\sl276\slmult1\f0\fs22\lang12 Hello world :3\par
}"; */
    }
}