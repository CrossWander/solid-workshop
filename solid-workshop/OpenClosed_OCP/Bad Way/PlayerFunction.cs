namespace solid_workshop.OpenClosed__OCP_.Bad_Way
{
    public enum MediaEngine
    {
        /// Windows Media Player engine
        WMP,
        /// NAudio engine
        NAudio,
        /// Bass engine
        Bass
    }

    public class PlayerFunction
    {
        public MediaEngine MediaEngine { get; set; }

        public PlayerFunction(MediaEngine mediaEngine)
        {
            MediaEngine = mediaEngine;
        }     

        public void Play()
        {
            if (MediaEngine == MediaEngine.WMP)
            {
                //Some code for play audio on Wmp engine
            }
            else if (MediaEngine == MediaEngine.NAudio)
            {
                //Some code for play audio on NAudio engine
            }
            else if (MediaEngine == MediaEngine.Bass)
            {
                //Some code for play audio on bass engine
            }
        }

        public void Pause()
        {
            if (MediaEngine == MediaEngine.WMP)
            {
                //Some code for pause audio on Wmp engine
            }
            else if (MediaEngine == MediaEngine.NAudio)
            {
                //Some code for pause audio on NAudio engine
            }
            else if (MediaEngine == MediaEngine.Bass)
            {
                //Some code for pause audio on Bass engine
            }
        }

        public void Stop()
        {
            if (MediaEngine == MediaEngine.WMP)
            {
                //Some code for stoop audio on Wmp engine
            }
            else if (MediaEngine == MediaEngine.NAudio)
            {
                //Some code for stoop audio on NAudio engine
            }
            else if (MediaEngine == MediaEngine.Bass)
            {
                //Some code for stoop audio on Bass engine
            }
        }
    }
}
