using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech; // Voice commands
using UnityEngine.VR.WSA.Input;  // Gestures

namespace UWB_RoomTexture
{
    public class Interactions : MonoBehaviour
    {
        public static KeywordRecognizer keywordRecognizer;
        public static Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
        public static GestureRecognizer gestureRecognizer;

        void Start()
        {
            keywordRecognizer = null;
            gestureRecognizer = new GestureRecognizer();
        }

        public static void EnableVoiceCommand(Constants.Commands.VoiceCommandsEnum command)
        {
            bool enabled = false;

            if (command == Constants.Commands.VoiceCommandsEnum.BeginRoomTexture)
            {
                // Command: Begin texture capture protocol 
                // (i.e. start up photocapture process)
                keywords.Add(Constants.Commands.Keyword_BeginRoomTexturing, () =>
                {
                    TextureCapture.BeginCapture();
                });

                enabled = true;
            }
            else if(command == Constants.Commands.VoiceCommandsEnum.RoomScreenshot)
            {
                // Command: Take texture screenshot
                keywords.Add(Constants.Commands.Keyword_RoomScreenshot, () =>
                {
                    TextureCapture.Capture();
                });
            }
            else if (command == Constants.Commands.VoiceCommandsEnum.EndRoomTexture)
            {
                // Command: End texture capture protocol and clean up
                keywords.Add(Constants.Commands.Keyword_EndRoomTexturing, () =>
                {
                    TextureCapture.EndCapture();
                });

                enabled = true;
            }

            if (enabled)
            {
                string[] keywordArray = new string[keywords.Keys.Count];
                keywords.Keys.CopyTo(keywordArray, 0);
                keywordRecognizer = new KeywordRecognizer(keywordArray);
                keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            }
        }

        public static void EnableVoiceCommand(IEnumerable<Constants.Commands.VoiceCommandsEnum> voiceCommands)
        {
            foreach (var command in voiceCommands)
            {
                EnableVoiceCommand(command);
            }
        }

        public static void DisableVoiceCommand(Constants.Commands.VoiceCommandsEnum command)
        {
            if (command == Constants.Commands.VoiceCommandsEnum.BeginRoomTexture)
                keywords.Remove(Constants.Commands.Keyword_BeginRoomTexturing);
            else if (command == Constants.Commands.VoiceCommandsEnum.EndRoomTexture)
                keywords.Remove(Constants.Commands.Keyword_EndRoomTexturing);

            if (keywords.Count == 0)
                keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
        }
        
        // ERROR TESTING 
        // public void enableGesture(s)


        // Handle logic for directing phrase recognition calls to the
        // appropriate method.
        private static void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            System.Action action;

            if (keywords.TryGetValue(args.text, out action))
                action.Invoke();
            else
                throw new System.Exception(Constants.ErrorStrings.HololensKeyphraseNotFound);

            // ERROR TESTING MUST CATCH THIS ERROR APPROPRIATELY
        }

        // Gesture commands




        // Add event logic to the keyphrase databank


        // Enable Hololens to recognize and act upon certain keywords
        // when this script is first enabled.
        public static void EnableGesture(Constants.Commands.GesturesEnum gesture)
        {
            if (gesture == Constants.Commands.GesturesEnum.RoomScreenshot)
            {
                gestureRecognizer.TappedEvent += GestureRecognizer_OnTappedEvent;
            }
            else
                throw new System.Exception(Constants.ErrorStrings.GestureNotFound);
        }

        public static void DisableGesture(Constants.Commands.GesturesEnum gesture)
        {
            if (gesture == Constants.Commands.GesturesEnum.RoomScreenshot)
            {
                gestureRecognizer.TappedEvent -= GestureRecognizer_OnTappedEvent;
            }
            else
                throw new System.Exception(Constants.ErrorStrings.GestureNotFound);
        }

        private static void GestureRecognizer_OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            TextureCapture.Capture();
        }

        // ERROR TESTING REMOVE
        /*
        void Start()
        {
            initialize();
        }

        // Handles the logic for initializing the keywords that will
        // be added to the Hololens databank. To add more commands, see
        // "initializeCommands" method.
        private void initialize()
        {
            string[] commands = initializeCommands();
            keywordRecognizer = new KeywordRecognizer(commands);
            enableKeywords(commands);
            keywordRecognizer.Start();
        }

        // Initialize commands to be added. This is where additional commands
        // must be inserted.
        private string[] initializeCommands()
        {
            string[] keywordArray = null;

            // Command: Begin texture capture protocol 
            // (i.e. start up photocapture process)
            keywords.Add(RoomTexture.Keyword_BeginRoomTexturing, () =>
            {
                TextureCapture.BeginCapture();
            });

            // Command: End texture capture protocol and clean up
            keywords.Add(RoomTexture.Keyword_EndRoomTexturing, () =>
            {
                TextureCapture.EndCapture();
            });

            // Generate array of all commands
            keywordArray = new string[keywords.Keys.Count];
            int keywordIndex = 0;
            foreach (string key in keywords.Keys)
            {
                keywordArray[keywordIndex++] = key;
            }

            return keywordArray;
        }
        
        public void enableKeywords(string[] keywordArray)
        {
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        }

        public void disableKeywords(string[] keywordArray)
        {
            keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
        }

        public void enableVoiceTrigger(string keyword)
        {
            keywordRecognizer.OnPhraseRecognized += 
        }

        public void disableVoiceTrigger(string keyword)
        {

        }
        */
    }
}