###################################################################################################################
###################################################################################################################
#################################################### README #######################################################
###################################################################################################################
###################################################################################################################

Dieser Webserver kann mit localhost:8080 aufgerufen werden.
Es können mit Hilfe von Reflections DLL-Dateiein (Plugins) dynamisch nachgeladen werden.
Diese müssen sich in dem Ordner Plugins am Webserver befindent.

Wird im Browser localhost:8080 aufgerufen, erscheint einer Übersichtsseite mit allen Plugins.
Durch Klick auf ein Plugin wird einer Übersichtsseite mit allen Verfügbaren Funktionen angezeigt.

###################################################################################################################

1	Beschreibung

1.1	FirstForm.cs
Es wird eine Auswahl aller verfügbaren Plugins an den Browser geschickt.

1.2	Html.cs
In dieser Klasse wird das HTML-Dokument zusammengestellt. 
Es werden CSS-Eigenschaften, JQuery, Javascript und der zu übermittelnde Inhalt zusammengeführt.

1.3	IPlugins.cs
Hier wird das Interface für die verschiedenen Plugins definiert.

1.4	LogFile.cs
Falls ein Fehler im Server auftritt, werden genauere Informationen in einem LogFile gespeichert. Diese sind jeweils mit Datum und Uhrzeit versehen.
Wenn das LogFile eine bestimmte Größe erreich (zur Zeit: 1MB) wird dieses in einer chronologischen Reihenfolge umbenannt und ein neues LogFile erstellt.

1.5	PluginManager.cs
Mit Hilfe von Reflections werden alle Plugins (von denen die DLL-Datei in einem bestimmten Ordner am Server gespeichert sind) geöffnet und überprüft, ob es sich um das gewünschte Plugin handelt. Dies geschieht dadurch, dass der Pluginname übergeben und mit dem Dateinamen der DLL-Datei verglichen wird. Ist das gewünschte Plugin vorhanden, werden weitere Funktionen im Plugin aufgerufen. 

1.6	Program.cs
Es wird die Klasse „Server“ aufgerufen und der Webserver gestartet. Das Programm wartet auf eine Eingabe. Geben Sie das Wort „Exit“ ein (es  kann in jeder beliebigen Form geschrieben werden), wird das Programm beendet.

1.7	Request.cs
Die Klasse unterscheidet zwischen einem GET-Request und einem POST-Request. Dementsprechend werden verschiedene Funktionen aufgerufen.

1.8	Response.cs
Diese Klasse wird aufgerufen, wenn etwas vom Server an den Browser geschickt werden soll. Es werden die eigentlichen Inhalte mit einem HTML-Grundgerüst und Headerinformationen versehen.

1.9	Server.cs
In dieser Klasse wird der Server in einem eigenen Thread gestartet. 
Beim Programmstart werden hier die Plugins „Temperatur“ und „Navi“ ohne Parameter gestartet. 
Dadurch wird ein Sensor ausgelesen und die Werte in eine Datenbank gespeichert. Des Weiteren wird ein OSM-File (OpenStreetMap) ausgelesen und intern abgespeichert.

1.10	Url.cs
Diese Klasse wird aufgerufen wenn eine Anfrage von einem Browser zu diesem Server stattgefunden hat und bereits bekannt ist, um welche Art von Anfrage es sich dabei handelt (GET, POST). Es werden dementsprechend verschiedene Funktionen in dieser Klasse aufgerufen um die Inhalte aufzuteilen und für die Plugins zugänglich zu machen.

1.11	WrongFilenameException.cs, WrongParameterException.cs
Diese zwei Klassen dienen dazu, genauere Aussagen über die Fehlerart treffen zu können. 

