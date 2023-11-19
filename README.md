
## Features

A simple IR library to index files and retrieve key words
using scoring algorithms.

## Example

Take a PDF, upload it and the system indexes the key-words/sentences for retrieval.

Example output:
```
1 || 1 || berkshire hathaway inc
1 || 2 ||  annual report
3 || 1 || berkshire hathaway inc
3 || 2 ||  annual report table of contents berkshires performance vs
3 || 3 || the sp
3 || 63 ||  chairmans letter
3 || 141 || - form-k businessdescription
3 || 212 || k- riskfactors
3 || 290 || k- description ofproperties
3 || 358 || k- managementsdiscussion
3 || 425 || k- managementsreport oninternalcontrols
3 || 479 || k- independentauditorsreport
3 || 543 || k- consolidatedfinancialstatements
3 || 603 || k- notes toconsolidatedfinancialstatements
3 || 656 || k- appendices  shareholderevent andmeetinginformation
3 || 709 || a- propertycasualtyinsurance
3 || 774 || a- operatingcompanies
3 || 845 || a- stocktransferagent
3 || 916 || a- directors andofficers ofthecompany
3 || 967 || insidebackcover bywarrenebuffett copyright allrightsreserved
4 || 1 || berkshires performance vs
4 || 2 || the sp  annual percentage change in per-share in sp  market value of with dividends berkshire included year
4 || 4236 ||   compoundedannualgain -
4 || 4281 ||   overallgain -
4 || 4337 ||   note data arefor calendaryears withthese exceptionsandyear endedmonths ended
5 || 1 || berkshire hathaway inc
5 || 2 || to the shareholders of berkshire hathaway inc charlie munger my long-time partner and i have the job of managing the savings of a great number of individuals
5 || 3 || we are grateful for their enduring trust a relationship that often spans much of their adult lifetime
5 || 4 || it is those dedicated savers that are forefront in my mind as i write this letter
5 || 5 || a common belief is that people choose to save when young expecting thereby to maintain their living standards after retirement
5 || 6 || any assets that remain at death this theory says will usually be left to their families or possibly to friends and philanthropy
5 || 7 || our experience has differed
```


### Classes

[x] Corpus
[x] Indexer
[x] Retriever
[x] Scorer
[x] Tokenizer


##  First priority

[x] Translate Corpus into Inverted Index.
[x] Cleanup Existing Code Base
    -   [x] Ensure class files and folder names are updated accordingly.
    -   [x] Ensure class files are not the same as namespaces for sake of using conventions.
[x] Update the Indexer class. <--


## Second Priority

[ ] Add S3 Capabilities.
[ ] Update Retriever class.
[ ] Build a Tokenizer class ( to tokenize ~ lemmatize and normalize each document )
