CREATE TABLE SchemaVersion(id STRING NOT NULL PRIMARY KEY );
CREATE TABLE ArrestReport (id STRING PRIMARY KEY, ArrestTime STRING NOT NULL, FirstName STRING NOT NULL, LastName STRING NOT NULL, DOB STRING NOT NULL, HomeAddress STRING, ArrestStreetAddress STRING, ArrestCity STRING, Details STRING );
CREATE TABLE ArrestReportLineItem (id STRING PRIMARY KEY, Charge STRING NOT NULL, FelonyLevel STRING NOT NULL, Note STRING, arrestReportId STRING, FOREIGN KEY(arrestReportId) REFERENCES ArrestReport(id) );
CREATE TABLE ArrestReportAdditionalParty (id STRING PRIMARY KEY, PartyType INTEGER, FirstName STRING NOT NULL, LastName STRING NOT NULL, DOB STRING NOT NULL, Details String, arrestReportId STRING, FOREIGN KEY(arrestReportId) REFERENCES ArrestReport(id) );
CREATE INDEX arrest_report_ped on ArrestReport (FirstName, LastName, DOB);
CREATE INDEX arrest_report_date on ArrestReport (ArrestTime);
CREATE INDEX arrest_report_additional_party_name on ArrestReportAdditionalParty (FirstName, LastName, DOB);
CREATE INDEX arrest_report_additional_party_report_id on ArrestReportAdditionalParty (arrestReportId);
CREATE INDEX arrest_report_line_items on ArrestReportLineItem (arrestReportId);