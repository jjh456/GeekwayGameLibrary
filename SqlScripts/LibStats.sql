USE [Library]

SELECT 
	game.[Title] Game,
	COUNT(DISTINCT chout.id) NumberOfTimesCheckedOut
FROM [dbo].[Games] game
JOIN [Copies] copy on copy.[GameID] = game.[ID]
JOIN CopyCollections coll on coll.ID = copy.CopyCollectionID
JOIN [Checkouts] chout on chout.[Copy_ID] = copy.[ID]

WHERE coll.ID = 1
	AND [TimeOut] BETWEEN '2021-10-04 18:00:00' AND '2021-10-7 18:00:00'

--[TimeOut] >= '2019-01-17 18:00:00' AND [TimeOut] <= '2019-01-21 08:00:00'
GROUP BY game.Title
ORDER BY game.Title ASC