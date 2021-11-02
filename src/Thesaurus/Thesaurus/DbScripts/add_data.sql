declare @meaningId int;

insert into Meanings ([Meaning]) values ('to end or arrive at a final stage');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'culminate', @meaningId;
insert into Words ([Word], [MeaningID]) select 'end', @meaningId;


insert into Meanings ([Meaning]) values ('someone or something that poses a danger to us');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'menace', @meaningId;
insert into Words ([Word], [MeaningID]) select 'threat', @meaningId;


insert into Meanings ([Meaning]) values ('something horrifying and repellent—especially acts and images of violence and death, with no gore spared');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'macabre', @meaningId;
insert into Words ([Word], [MeaningID]) select 'gruesome', @meaningId;


insert into Meanings ([Meaning]) values ('the act of misleading, tricking, or deceiving someone, usually for one’s own advantage or gain');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'subterfuge', @meaningId;
insert into Words ([Word], [MeaningID]) select 'deception', @meaningId;


insert into Meanings ([Meaning]) values ('describe things that are conspicuously showy and intended to attract attention and impress');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'gaudy', @meaningId;
insert into Words ([Word], [MeaningID]) select 'flashy', @meaningId;


insert into Meanings ([Meaning]) values ('a person or thing that is so different from the norm that they appear strange or are difficult to explain');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'anomaly', @meaningId;
insert into Words ([Word], [MeaningID]) select 'oddity', @meaningId;


insert into Meanings ([Meaning]) values ('a boundary where a surface or area ends');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'periphery', @meaningId;
insert into Words ([Word], [MeaningID]) select 'edge', @meaningId;


insert into Meanings ([Meaning]) values ('things that happen infrequently and not at regular intervals');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'sporadic', @meaningId;
insert into Words ([Word], [MeaningID]) select 'occasional', @meaningId;


insert into Meanings ([Meaning]) values ('someone who puts on a false or deceptive appearance or persona');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'charlatan', @meaningId;
insert into Words ([Word], [MeaningID]) select 'fake', @meaningId;


insert into Meanings ([Meaning]) values ('a person or thing widely known and (usually) admired');

set @meaningId = @@IDENTITY;

insert into Words ([Word], [MeaningID]) select 'renowned', @meaningId;
insert into Words ([Word], [MeaningID]) select 'famous', @meaningId;
