using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading.Tasks;

using IRLib.IRIndexer.Enums;
using IRLib.IRIndexer.Interfaces;

namespace IRLib.IRIndexer
{
    public class IndexerInvoker : IIterator
    {
        List<IndexerComponent> components;
        Indexer indexer;
        int position = 0;

        public IndexerInvoker(Indexer indexer)
        {
            this.components = new List<IndexerComponent>();
            this.indexer = indexer;
            this.BuildComponents();
        }

        public void BuildComponents()
        {
            if(this.indexer == null)
                throw new Exception("No indexer is provided.");


            if(this.indexer.GetType() == typeof(SFIndexer))
            {
                SFIndexer _indexer = (SFIndexer)this.indexer;
                // switch on the file destination
                switch(_indexer.fileDestination)
                {
                    case FileDestination.Local: 

                        // check what the file type is.
                        if(_indexer.fileType == FileType.PDF)
                        {
                            // build the localized invoker
                            this.components.Add(new IndexerComponent(EIndexerComponent.BUILDDESTINATIONFOLDER)); // build the folder
                            this.components.Add(new IndexerComponent(EIndexerComponent.PARSEMEMORYSTREAM)); // parse pdf 
                            this.components.Add(new IndexerComponent(EIndexerComponent.WRITESENTENCES)); // write the file
                        }
                        return;

                    default: 
                        throw new Exception("No file destination type defined in the indexer.");

                }
            }

            if(this.indexer.GetType() == typeof(BSBIndexer))
            {
                BSBIndexer _indexer = (BSBIndexer)this.indexer;
                this.components.Add(new IndexerComponent(EIndexerComponent.VALIDATESOURCEFOLDER));
                this.components.Add(new IndexerComponent(EIndexerComponent.BUILDINDEX));
                this.components.Add(new IndexerComponent(EIndexerComponent.WRITEINDEXTOFILE));
            }


        }


        public void Fire()
        {
            IEnumerator iterator = this.components.GetEnumerator();
            while(iterator.MoveNext() && iterator.Current != null)
            {
                IndexerComponent ic = (IndexerComponent)iterator.Current;
                if(this.indexer.GetType() == typeof(SFIndexer))
                {
                    SFIndexer _indexer = (SFIndexer)this.indexer;
                    // fire on the method type
                    switch(ic.GetComponent())
                    {
                        case EIndexerComponent.BUILDDESTINATIONFOLDER: 
                            _indexer.BuildDestinationFolder();
                            break;
                        case EIndexerComponent.PARSEMEMORYSTREAM: 
                            _indexer.ParseMemoryStream();
                            break;
                        case EIndexerComponent.WRITESENTENCES: 
                            _indexer.WriteSentences();
                            break;
                        default: 
                            throw new Exception("No enum defined in the indexer builder.");

                    }
                }

                if(this.indexer.GetType() == typeof(BSBIndexer))
                {
                    BSBIndexer _indexer = (BSBIndexer)this.indexer;
                    switch(ic.GetComponent())
                    {
                        case EIndexerComponent.VALIDATESOURCEFOLDER: 
                            _indexer.ValidateSourceFolder();
                            break;
                        case EIndexerComponent.BUILDINDEX: 
                            _indexer.BuildIndex();
                            break;
                        case EIndexerComponent.WRITEINDEXTOFILE: 
                            _indexer.WriteIndexToFile();
                            break;
                        default: 
                            throw new Exception("Not a valid index component.");
                    }
                }

            }
        }


        #region not used atm.
        public IndexerComponent MoveNext()
        {
            IndexerComponent c = components[position]; // set the 
            position++;
            return c;
        }

        public bool HasNext()
        {
            if(this.position >= components.Count || this.components[position] == null)
                return false;
            return true;
        }
        #endregion

    }
}