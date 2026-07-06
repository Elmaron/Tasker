# Tasker
Ein Aufgaben-Planner und Tracker, der dir hilft, dein Arbeitsverhalten zu verstehen.

**ACHTUNG! Dieses Projekt ist in seinem aktuellem Zustand noch nicht zur Verwendung bereit. Klicke [hier](#aktuell) für mehr Informationen.**

*This is the german translation. You can find the readme in english [here](https://github.com/Elmaron/Tasker).*

# Entwicklungsstatus
## Aktuell
*Das Programm hat aktuell keine verwendbare Version.*
Nachdem ich das grundlegende Design des Programms überarbeitet habe, werde ich mich mit den Grund-Funktionen und der Datenbank beschäftigen, damit ich eine erste Alpha-Version veröffentlichen kann.
Folgt mir gerne auf Github, um Updates zum Projekt zu bekommen!

Status: **[Alpha](#alpha)** | [Beta](#beta) | [Release](#veröffentlichung-v10)
*abgeschlossen*; **in Bearbeitung**; Nächste Phase

# Funktionen
Dieses Aufgaben-Planungs-Programm verwendet eine lokale Datenbank, um deine Aufgaben zu verfolgen. Die Daten, die dabei gesammelt werden, benutzt das Programm, um deine zukünftigen Aufgaben und Termine zu planen, falls du diese schon einmal erledigt hast.

Das Programm soll auch Menschen helfen, die es schwierig finden, eine Aufgabe "einfach zu tun", mit einer Aufgabe anzufangen und produktiv zu werden, falls das ein Problem ist. Trotzdem sind das nur Empfehlungen, die dir mehr Perspektiven geben sollen, um bei Dingen aktiver werden zu können, die "getan werden müssen".

Am Ende soll das Programm dem Benutzer helfen, sich zwischen Aufgaben zu entscheiden, die getan werden müssen und diese zu tun.

## Eine Stellungnahme zum Thema KI
Grundsätzlich bin ich *nicht* gegen neue Technologien. Das gilt auch für das Thema KI. **Allerdings** finde ich die Verwendung von KI in den meisten Fällen überflüssig und unangemessen. Deshalb möchte ich hier erklären, *wie* und *ob* ich KI während der Entwicklung einsetze und was man in Zukunft erwarten kann, wenn KI in Verbindung mit dem Programm genutzt wird.

### Entwicklung
Während der Entwicklung kann KI definitiv die Produktivität erhöhen, wenn diese Teile des Codes selbst schreibt. Das ist ein Fakt, den ich **nicht** widerlegen kann. Trotzdem mag ich die Idee nicht, dass *etwas* anderes meinen Code schreibt. Stattdessen benutze ich KI für die folgenden Dinge bei der Entwicklung:
* Um neue Befehle zu erlernen
* Um einen Befehl zu finden, von dem ich nicht wusste, dass es ihn gibt
* Um die Basics von etwas neuem zu verstehen (bspw. Befehlen, Bibliotheken, etc.)
* Um den Grund für ein Problem zu finden, den ich nicht finden kann

Wenn ich etwas neues lernen muss, um bspw. die Avalonia Bibliothek zu nutzen, schaue ich mir zuerst die Dokumentation an. Wenn ich nach einer Weile nicht in der Lage dazu bin, die Konzepte zu verstehen, benutze ich KI als Hilfe. Oft lerne ich über diesen Weg andere, mir unbekannte Befehle und/oder Bibliotheken kennen. Also wiederhole ich den Prozess, bis ich genug verstehe, um die Konzepte anwenden zu können.
Falls ich bei einem Problem einen Fehler bekomme, bei dem ich den Grund des Fehlers nicht nachvollziehen kann, frage ich ggf. KI, um das Problem identifizieren zu können. Bevor ich den Code jedoch an die KI übergebe, versuche ich den gesamten Code umzuschreiben, damit die KI nicht so einfach nachvollziehen kann, was ich genau mache (bspw. benenne ich Variablen um; schicke der Ki nur, was sie braucht; usw.) Leider schreibt KI in der Antwort immer die Lösung für das Problem mit hinzu, selbst wenn ich nicht danach frage. Darum ignoriere ich sie meistens. Ich kopiere nie einfach den Code der KI. Falls ich Code-Ausschnitte benutze, schaue ich sie mir zuerst genau an, bis ich diese verstanden habe und tippe sie dann in mein eigenes Programm selbst ein. Ich verwende KI nicht, um meinen Code zu verbessern.
Ich könnte KI natürlich mehr benutzen, so wäre ich bei allem deutlich schneller. Ich möchte jedoch alles verstehen, was ich tue. Genauso wenig möchte ich dümmer werden. Deshalb benutze ich KI, wie ich es eben tue. Ich werde immer versuchen, die Fehler selbst zu verstehen, bevor ich darüber nachdenke, eine KI anzufragen. In Zukunft möchte ich mich auch deutlich mehr auf Stackoverflow oder ähnliches verlassen.
Falls du mehr über dieses Thema wissen möchtest, kannst du mir gerne eine [Mail](mailto:info@vindona.de) schreiben (schnellste Antwort).

### KI Assistent
Grundsätzlich ich finde die Idee eines KI Assistenten **nicht** verwerflich. **Aber nicht** als "Ich kann dich alles fragen und du machst das"-Programm. Ich finde die Idee, ein anderes KI-Programm zu benutzen oder einen Server oder irgendetwas außerhalb für anzuschreiben, ebenfalls schlecht. **Aktuell plane ich nicht, einen KI Assistenten in das Programm einzubauen.**
Falls ich es doch machen sollte, dann sind hier die Dinge, die du von dem Assistenten erwarten und nicht erwarten kannst: 

* Der KI Assistent wird entweder ins Programm integriert oder als Plugin verfügbar sein.
* Der Assistent kann komplett ausgeschaltet werden.
* Die KI ist nur lokal verfügbar und kann sich nicht mit anderen KIs in Verbindung setzen.
    * Falls du eine selbstgehostete Version für Browser verwendest, läuft der Assistent nur auf deinem Server und kann sich nicht mit anderen KIs in Verbindung setzen.
* Du wirst immer transparente Informationen dazu bekommen, was der KI Assistent macht und was nicht.
    * Der Assistent ist als Alternative zum Algorithmus verfügbar, um deine Zeiten bei Aufgaben besser zu analysieren.
    * Der Assistent kann die Aufgaben (für verschiedene Tage) vorschlagen, aber wird sie nicht für dich planen.
    * Der Assistent kann **nicht** den Inhalt deiner Aufgaben analysieren, um die auf dieser Basis neue Aufgaben vorzuschlagen, falls du noch welche gebrauchen könntest.
    * Du kannst nicht in einem Chat mit der KI interagieren.

## Geplante Inhalte
### Alpha
- [x] Github Projekt erstellen
- [ ] Design überarbeiten
    - [ ] Layout und Design neu entwerfen
- [ ] Erstelle die Grundfunktionen
    - [ ] Lese- und Schreibfunktionen für die Datenbank (durch SQLite-Befehle)
    - [ ] Erstelle Klassen und Befehle für die Interaktion mit der Datenbank
    - [ ] Erstelle Layout
    - [ ] Verknüpfe das Layout mit den Befehlen und Klassen
- [ ] Überarbeite den Code
- [ ] Behebe Fehler


### Beta
*Mehr Punkte könnten hinzugefügt werden, während ich an dem Programm arbeite.*
- [ ] Design-Funktionen überarbeiten
    - [ ] Erstelle wechselbare Designs
- [ ] Neue Funktionen
    - [ ] Daten exportieren
    - [ ] Algorithmus, um beim planen neuer Aufgaben und Termine
    - [ ] Veränderbare Einstellungen
        - [ ] Tastaturbefehle
        - [ ] Automatisches Löschen der lokalen Daten
            - [ ] Nach einer bestimmten Zeit
            - [ ] Nachdem die Daten eine bestimmte Größe auf der Festplatte erreicht haben
            - [ ] Deaktiveren des automatischen Löschens von Daten
- [ ] Optimierungen für verschiedene Benutzerarten
    - [ ] Standard
    - [ ] Menschen mit ADHS
- [ ] Überarbeite den Code
- [ ] Behebe mehr Fehler

### Veröffentlichung (V1.0)
*Mehr Punkte könnten hinzugefügt werden, während ich an dem Programm arbeite.*
- [ ] Design-Funktionen überarbeiten
    - [ ] Ermögliche eigene Designs
- [ ] Neue Funktionen
    - [ ] Plugins
- [ ] Funktions-Updates
    - [ ] Verbesserungen am Algorithmus
- [ ] Erstelle Benutzerhilfen
    - [ ] Online Benutzerhilfe (detailiert)
    - [ ] Eingebaute Hilfe für Anfänger
- [ ] ERstelle Dokumentationen
    - [ ] Für Entwickler
        - [ ] Programm-/Appentwicklung
        - [ ] Pluginentwicklung
    - [ ] Für Designer
- [ ] Überarbeite den Code
- [ ] Behebe noch mehr Fehler

## Planung für nach der Veröffentlichung
*Im Moment sind dies nur Ideen, nichts ist versprochen.*
- [ ] Selbstgehostete Version für den Browser
- [ ] Server Synchronisation für die Datenbank
- [ ] KI Assistent für neue Aufgaben (nur auf deinem Gerät)
- [ ] Ein paar lustige Belohnungen für das erfüllen deiner Aufgaben!
    - [ ] Du kannst selbst welche erstellen oder das Programm dir welche empfehlen lassen
    - [ ] Das Programm hat eingebaute kleine Mini-Spiele, die belohnend sein sollen

*Dieser Text wurde ohne die Verwendung von AI geschrieben.*
*Letzte Veränderung zu Read Me: 02.07.2026*
**dieses Projekt wird von Elmaron (Vindona) entwickelt**
