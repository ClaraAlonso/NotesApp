using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace NotesApp.ViewModel
{
    public class SpeechHelper
    {
        private const string LANGUAGE = "pt-BR";
        private const string REGION_AZURE = "eastus";
        private const string KEY_AZURE = "45e0efe0ea7d47eb84a2ae07f4549000";
        public SpeechHelper() { }

        public static async Task<SpeechRecognitionResult> SpeechToTextAzureAsync() 
        {
            var speechConfig = SpeechConfig.FromSubscription(KEY_AZURE, REGION_AZURE);
            speechConfig.SpeechRecognitionLanguage = LANGUAGE;

            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
            {
                return await recognizer.RecognizeOnceAsync();
            }
        }

        /* Speech using System.Speech
        SpeechRecognitionEngine recognizer; 
        bool isRecognizing =  false;
        */

        /* Speech using System.Speech
             * 
            recognizer = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers().FirstOrDefault());

            GrammarBuilder builder = new GrammarBuilder();
            builder.AppendDictation();
            Grammar grammar = new Grammar(builder);

            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        */

        /* Speech using System.Speech
         * 
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;

            contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(recognizedText)));
        }
        */

        /* Speech using System.Speech

            if (!isRecognizing)
            {
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
                isRecognizing = true;
            }
            else 
            { 
                recognizer.RecognizeAsyncStop();
                isRecognizing = false;
            }
            */
    }
}
