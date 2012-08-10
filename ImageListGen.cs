#region File Description
//-----------------------------------------------------------------------------
// ImageListGen.cs
//
// Generates a list of pictures 
// 
// 
// 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
#endregion

namespace PicViewerWPF
{
    class ImageListGen
    {
        private int picsPerDir;
        private DirectoryInfo dir;
        private Random rand;
        private Dictionary<string, int> dupesTestDict;

        /// <summary>
        /// ImageListGen
        /// </summary>
        public ImageListGen()
        {
            rand = new Random();
            dupesTestDict = new Dictionary<string, int>();
        }

        /// <summary>
        /// generateImageList- generates a list of pictures
        /// </summary>
        /// <param name="srcDir">Directory to get the pictures from</param>
        /// <param name="maxImageLstLen">the set size</param>
        /// <returns></returns>
        public List<string> generateImageList(string srcDir, int maxImageLstLen)
       {
           DirectoryInfo[] subdirs;

           dir = new DirectoryInfo(srcDir);
           subdirs = dir.GetDirectories();
            
            /*
           foreach (DirectoryInfo subdir in subdirs)
           {
               Debug.WriteLine( "Subdir Full Name: " +subdir.FullName  );
               Debug.WriteLine("Subdir Name: " + subdir.Name);
           }
             */

           return (makeFileList(dir, subdirs, maxImageLstLen));
       }
        
        /// <summary>
        /// makeFileList- actually makes the list
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="subdirs"></param>
        /// <param name="iMax"></param>
        /// <returns></returns>
       private List<string> makeFileList( DirectoryInfo srcdir,DirectoryInfo[] subdirs,int iMax)
       {
           FileInfo[] filesInfo;
           List<string> filelist;
           List<string> outfilelist;

           filelist = new List<string>();
           outfilelist = new List<string>();
           
           //check top level dir
           //parent dir may have files besides subdirs
          filesInfo= srcdir.GetFiles();
          foreach (FileInfo fileinfo in filesInfo)
          {
              //Debug.WriteLine("file Full Name: " + fileinfo.FullName);
              //Debug.WriteLine("file Name: " + fileinfo.Name);

              if (checkFiles(fileinfo.FullName) == true)
                  filelist.Add(fileinfo.FullName);
          }

           foreach (DirectoryInfo subdir in subdirs)
           {
               filesInfo = subdir.GetFiles();
               //Debug.WriteLine("_________________________");
               foreach (FileInfo fileinfo in filesInfo)
               {
                   //Debug.WriteLine("file Full Name: " + fileinfo.FullName);
                   //Debug.WriteLine("file Name: " + fileinfo.Name);

                   if (checkFiles(fileinfo.FullName)==true)
                        filelist.Add(fileinfo.FullName);
               }

               dupesTestDict.Clear();//reset dupes dir

               //Debug.WriteLine("_________________________");
           }

           outfilelist = pickFiles(filelist, iMax);
           return (outfilelist);
   }
        
        /// <summary>
       /// checkFiles- have to check the file types because the windows image viewer control will crash if its a
       /// non image file.
        /// </summary>
        /// <param name="testFilename"></param>
        /// <returns></returns>
       private bool checkFiles(string testFilename)
       {
           string stemp;
           stemp = testFilename.ToLower();
           if (stemp.Contains(".swf") == true)
               return (false);
           else if (stemp.Contains(".mp4") == true)
               return (false);
            else if (stemp.Contains(".html") == true)
               return (false);
           else if (stemp.Contains(".db") == true)
               return (false);
           else if (stemp.Contains(".txt") == true)
               return (false);
           else //check for dupes, will not work for renamed dupes
           {
               //Dupe finding is a fail
               // 2 files can have the same name and not be dupes
               // 2 files can have the same name and be dupes
               // 2 files can have the same content but one could be bigger or have a border.
              // if (dupesTestDict.Count == 0 || dupesTestDict.ContainsKey(stemp) == false)//not in list, save it
              // {
                //   dupesTestDict.Add(stemp, 1);
                   return (true);//not in list, not a dupe
               //}
              // else
              // {
                //   return (false);//found in list already, is a dupe
               //}
           }
       }

        /// <summary>
       /// pickFiles- goes through the list and picks out a set.
        /// </summary>
        /// <param name="filelist"></param>
        /// <param name="iMax"></param>
        /// <returns></returns>
       private List<string> pickFiles(List<string> filelist, int iMax)
       {
           List<string> outfilelist;
           int a;
           int index;
           int actualMax;// sometimes there is not enough in the dirs to cover the max.
           outfilelist = new List<string>();
           bool bRand;

           //if there are not enough to go in a set, just read them all
           // good for comics
           if (filelist.Count < iMax)
           {
               actualMax = filelist.Count;//less files than the max requested go with lower number
               for (a = 0; a < actualMax; a++)
               {
                   outfilelist.Add(filelist[a]);
               }
           }
           else
           {// plenty of content, get a set's worth
               actualMax = iMax;
               for (a = 0; a < actualMax; a++)
               {
                   index = rand.Next(filelist.Count);

                   outfilelist.Add(filelist[index]);
                   filelist.RemoveAt(index);//remove it so we do not see it again at the same place
               }
           }

           return (outfilelist);
       }



    }
}
