USE [Library]

SELECT 
	game.[Title] Game,
	COUNT(chout.id) TimesCheckedOut
FROM [dbo].[Games] game 
JOIN [Copies] copy on copy.[GameID] = game.[ID]
JOIN [Checkouts] chout on chout.[Copy_ID] = copy.[ID]

WHERE [TimeOut] BETWEEN '2018-12-31 18:00:00' AND '2019-12-31 18:00:00'

GROUP BY game.Title
ORDER BY TimesCheckedOut, game.Title ASC