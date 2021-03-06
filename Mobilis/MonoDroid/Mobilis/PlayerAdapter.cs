using Mobilis.Lib;
using Android.Media;
using System.Threading;
using Mobilis.Lib.Util;
using System;
using Java.IO;

namespace Mobilis
{
    public class PlayerAdapter : MediaPlayer,IAsyncPlayer, MediaPlayer.IOnCompletionListener,MediaPlayer.IOnErrorListener
    {
        public Mobilis.Lib.TTSManager.BlockFinishedPlaying callback;

        /*Implementa��o do player de posts na plataforma Android 
          
         * Os posts estar�o localizados na pasta Mobilis/TTS/:(id do post)
         * onde haver� um arquivo para cada bloco.
         */

        public PlayerAdapter() 
        {
            this.SetOnCompletionListener(this);
            this.SetOnErrorListener(this);
        }

        public void play(int blockId,Mobilis.Lib.TTSManager.BlockFinishedPlaying callback) 
        {
            this.callback = callback;
            base.Reset();
            File playingFile = new File(Constants.RECORGING_PATH + blockId + Constants.AUDIO_FILE_EXTENSION);
            base.SetDataSource(playingFile.AbsolutePath);
            base.SetVolume(100, 100);
            base.Prepare();
            base.Start();
        }

        public void stop() 
        {
            base.Stop();
        }

        public void OnCompletion(MediaPlayer mp)
        {
            System.Diagnostics.Debug.WriteLine("Finished player calling callback");
            callback();
        }

        public bool OnError(MediaPlayer mp, MediaError what, int extra)
        {
            System.Diagnostics.Debug.WriteLine("MediaPlayer on ErrorListener");
            return true;
        }

        public void reset() 
        {
            base.Reset();
        }
    }
}