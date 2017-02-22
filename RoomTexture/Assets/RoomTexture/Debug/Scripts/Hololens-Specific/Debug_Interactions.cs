using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UWB_RoomTexture
{
    namespace UWB_Debug
    {
        public class Debug_Interactions : MonoBehaviour
        {
            public string EnableVoiceCommand;
            public string DisableVoiceCommand;
            public string EnableGesture;
            public string DisableGesture;

            void Start()
            {
                bool enableVoiceCommand = Test_EnableVoiceCommand();
                bool disableVoiceCommand = Test_DisableVoiceCommand();
                bool enableGesture = Test_EnableGesture();
                bool disableGesture = Test_DisableGesture();
                
                string pass = Debug_RoomTexture.Debug_TestPassedString;
                string fail = Debug_RoomTexture.Debug_TestFailString;

                EnableVoiceCommand = (enableVoiceCommand) ? pass : fail;
                DisableVoiceCommand = (disableVoiceCommand) ? pass : fail;
                EnableGesture = (enableGesture) ? pass : fail;
                DisableGesture = (disableGesture) ? pass : fail;
            }


            public static bool Test_EnableVoiceCommand()
            {
                if (Debug_RoomTexture.Flag_Info)
                {
                    Debug.Log("Testing \"EnableVoiceCommand\" method.");
                }

                bool passed = true;

                if (Interactions.keywords.Count != 0)
                {
                    passed = false;
                }
                else
                {
                    Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.BeginRoomTexture);
                    if (!Interactions.keywords.ContainsKey(Constants.Commands.Keyword_BeginRoomTexturing))
                        passed = false;
                    Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.RoomScreenshot);
                    if (!Interactions.keywords.ContainsKey(Constants.Commands.Keyword_RoomScreenshot))
                        passed = false;
                    Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.EndRoomTexture);
                    if (!Interactions.keywords.ContainsKey(Constants.Commands.Keyword_EndRoomTexturing))
                        passed = false;

                    if (Interactions.keywords.Count != 3)
                        passed = false;

                    Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.BeginRoomTexture);
                    Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.RoomScreenshot);
                    Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.EndRoomTexture);
                }

                if (!passed)
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);

                return passed;
            }

            public static bool Test_DisableVoiceCommand()
            {
                if (Debug_RoomTexture.Flag_Info)
                {
                    Debug.Log("Testing \"DisableVoiceCommand\" method.");
                }

                bool passed = true;

                if (Interactions.keywords.Count != 0)
                {
                    passed = false;
                }
                else
                {
                    Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.BeginRoomTexture);
                    if (!Interactions.keywords.ContainsKey(Constants.Commands.Keyword_BeginRoomTexturing))
                        passed = false;
                    Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.RoomScreenshot);
                    if (!Interactions.keywords.ContainsKey(Constants.Commands.Keyword_RoomScreenshot))
                        passed = false;
                    Interactions.EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum.EndRoomTexture);
                    if (!Interactions.keywords.ContainsKey(Constants.Commands.Keyword_EndRoomTexturing))
                        passed = false;

                    if (Interactions.keywords.Count != 3)
                        passed = false;

                    Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.BeginRoomTexture);
                    Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.RoomScreenshot);
                    Interactions.DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum.EndRoomTexture);

                    if (Interactions.keywords.Count != 0)
                        passed = false;
                }

                if (!passed)
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);

                return passed;
            }

            public static bool Test_EnableGesture()
            {
                if (Debug_RoomTexture.Flag_Info)
                {
                    Debug.Log("Testing \"EnableGesture\" method.");
                }

                bool passed = true;

                if (Interactions.gestureRecognizer == null)
                    passed = false;
                else
                {
                    try {
                        Interactions.EnableGesture(Constants.Commands.GesturesEnum.RoomScreenshot);
                    }
                    catch(System.Exception e)
                    {
                        if (e.Message == Constants.ErrorStrings.GestureNotFound)
                            passed = false;
                    }

                    try
                    {
                        Interactions.DisableGesture(Constants.Commands.GesturesEnum.RoomScreenshot);
                    }
                    catch (System.Exception e) { }
                }
                
                if (!passed)
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);

                return passed;
            }

            public static bool Test_DisableGesture()
            {
                if (Debug_RoomTexture.Flag_Info)
                {
                    Debug.Log("Testing \"DisableGesture\" method.");
                }

                bool passed = true;

                if (Interactions.gestureRecognizer == null)
                    passed = false;
                else
                {
                    try
                    {
                        Interactions.EnableGesture(Constants.Commands.GesturesEnum.RoomScreenshot);
                    }
                    catch (System.Exception e) { }

                    try
                    {
                        Interactions.DisableGesture(Constants.Commands.GesturesEnum.RoomScreenshot);
                    }
                    catch (System.Exception e)
                    {
                        if (e.Message == Constants.ErrorStrings.GestureNotFound)
                            passed = false;
                    }
                }

                if (!passed)
                    Debug.Log(Debug_RoomTexture.Debug_TestFailString);

                return passed;
            }
        }
    }
}