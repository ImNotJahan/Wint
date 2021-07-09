﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace IniFiles
{
    public class Keybinds : MonoBehaviour
    {
        private static int version = 2;

        //Movement
        public static string forward = "w";
        public static string backward = "s";
        public static string left = "a";
        public static string right = "d";
        public static string crouch = "left ctrl";
        public static string sprint = "left shift";
        public static string jump = "space";

        //Attacking
        public static string focus = "f";
        public static string style = "q";
        public static string interact = "e";

        //Multiplayer
        public static string chat = "return";

        private static string directorypath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                @"\Jahan Rashidi\Familia Game\Settings\";
        public static void LoadKeybinds()
        {
            Ini keybinds = new Ini(directorypath + "Keybinds.ini");
            if (!keybinds.KeyExists("Version") || int.Parse(keybinds.Read("Version")) < version)
            {
                SaveKeybinds();
            }
            else
            {
                forward = keybinds.Read("Forward", "Movement");
                backward = keybinds.Read("Backward", "Movement");
                left = keybinds.Read("Left", "Movement");
                right = keybinds.Read("Right", "Movement");
                crouch = keybinds.Read("Crouch", "Movement");
                sprint = keybinds.Read("Sprint", "Movement");
                sprint = keybinds.Read("Jump", "Movement");

                focus = keybinds.Read("Focus", "Attacking");
                interact = keybinds.Read("Interact", "Attacking");
                style = keybinds.Read("Style", "Attacking");

                style = keybinds.Read("Chat", "Multiplayer");
            }
        }

        public static void SaveKeybinds()
        {
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }
            Ini keybinds = new Ini(directorypath + "Keybinds.ini");

            keybinds.Write("Version", version.ToString());

            keybinds.Write("Forward", forward, "Movement");
            keybinds.Write("Backward", backward, "Movement");
            keybinds.Write("Left", left, "Movement");
            keybinds.Write("Right", right, "Movement");
            keybinds.Write("Crouch", crouch, "Movement");
            keybinds.Write("Sprint", sprint, "Movement");
            keybinds.Write("Jump", jump, "Movement");

            keybinds.Write("Focus", focus, "Attacking");
            keybinds.Write("Style", style, "Attacking");
            keybinds.Write("Interact", interact, "Attacking");

            keybinds.Write("Chat", chat, "Multiplayer");
        }
    }
    
    //Ini class copied from Danny Beckett
    class Ini
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public Ini(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}