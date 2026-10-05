using NAudio.Wave;

namespace Classes
{
    public class Horse
    {
        public void Create(string outputDirectory)
        {
            int sampleRate = 44100;
            double duration = 1.0;
            int totalSamples = ((int)(sampleRate * duration));
            float[] buffer = new float[totalSamples];
            Random random = new Random();

            /*
            4-beat galloping rhythm pattern
            in a full racing gallop, a horse completes an entire stride cycle in about 400 to 440 milliseconds
            this should align well with 120 bpm with nice syncopation
            and we can always splice and rearrange in the daw if needed
	        beat 1 to beat 2 (left hind to right hind): ~60 milliseconds
	        beat 2 to beat 3 (right hind to left front): ~70 milliseconds
	        beat 3 to beat 4 (left front to right front/lead leg): ~70 milliseconds
	        beat 4 back to beat 1 (right front to next left hind): ~230 milliseconds
            */
            double[] strikeTimes = { 
                0.06, 0.13, 0.2, 0.43
            };

            foreach (double strikeTime in strikeTimes)
            {
                int startSample = ((int)(strikeTime * sampleRate));
                int hoofDuration = ((int)(0.08 * sampleRate)); // total length ~80ms
                double sinePhase = 0;

                for (int i = 0; i < hoofDuration && (startSample + i) < totalSamples; i++)
                {
                    float time = ((float)i) / sampleRate;

                    // thud (pitch sweep / drum modeling)
                    // starts high (180Hz) and drops instantly to bass (20Hz)
                    double currentFrequency = 20.0 + (160.0 * Math.Exp(-150.0 * time));
                    
                    sinePhase += (2.0 * Math.PI * currentFrequency) / sampleRate;
                    
                    float thudWave = ((float)Math.Sin(sinePhase));

                    // thud volume envelope decays quickly over 40ms
                    float thudEnvelope = MathF.Exp(-60.0f * time);
                    float finalThud = thudWave * thudEnvelope * 0.7f;

                    // clack (transient click)
                    // white noise that silences almost immediately (gone in 6ms)
                    float rawNoise = ((float)(random.NextDouble() * 2.0 - 1.0));
                    float clackEnvelope = MathF.Exp(-700.0f * time); // extreme rapid decay
                    float finalClack = rawNoise * clackEnvelope * 0.2f;

                    // mix
                    buffer[startSample + i] += finalThud + finalClack;
                }
            }

            string outputPath = Path.Combine(outputDirectory, "horse.wav");
            // 32-bit float, mono (we'll set their spacial placements in the daw)
            WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1); 
            
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputPath, waveFormat))
            {
                waveFileWriter.WriteSamples(buffer, 0, buffer.Length);
                waveFileWriter.Flush();
            }
        }
    }
}