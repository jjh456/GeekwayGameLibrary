declare @copiesInColl table (id int);
insert @copiesInColl
	SELECT ID FROM Copies c WHERE c.CopyCollectionID = 4

declare @gamesInColl table (id int);
insert @gamesInColl
	SELECT g.ID
	FROM Copies c
		JOIN Games g on g.ID = c.GameID
	WHERE c.CopyCollectionID = 4

--DELETE FROM [dbo].Copies WHERE ID in (select id from @copiesInColl)
--DELETE FROM [dbo].Games WHERE ID in (select id from @gamesInColl)
