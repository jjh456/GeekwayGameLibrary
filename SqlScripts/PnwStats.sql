USE [Library]

SELECT 
	game.[Title] Game,
	COUNT(DISTINCT chout.id) NumberOfTimesCheckedOut,
	ROUND(AVG(CAST(rating.[Value] as float)),2) AvgRating,
	ROUND(ROUND(AVG(CAST(player.WantsToWin as float)),2) * COUNT(DISTINCT chout.id), 0) NumWantToWin
FROM [dbo].[Games] game
JOIN [Copies] copy on copy.[GameID] = game.[ID]
JOIN CopyCollections coll on coll.ID = copy.CopyCollectionID
JOIN [Checkouts] chout on chout.[Copy_ID] = copy.[ID]
LEFT JOIN Ratings rating on rating.Game_ID = game.ID
LEFT JOIN Players player on player.ID = rating.ID

WHERE coll.ID = 3
	AND [TimeOut] BETWEEN '2021-10-04 18:00:00' AND '2021-10-7 18:00:00'


--[TimeOut] >= '2019-01-17 18:00:00' AND [TimeOut] <= '2019-01-21 08:00:00'
GROUP BY game.Title, Game_ID
ORDER BY game.Title ASC