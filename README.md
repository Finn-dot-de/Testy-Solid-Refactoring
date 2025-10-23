# ActiveLog - Trainingstagebuch

## 📧 E-Mail vom Product Owner

```
Von: Sarah Meyer <s.meyer@fittech.de>
An: Entwicklerteam <dev-team@fittech.de>
Betreff: ActiveLog: Dringende Erweiterungen für Q1 2026
Datum: 04.10.2025

Hallo Team,

gute Neuigkeiten! 🎉 ActiveLog hat in den letzten Wochen über 500 aktive Nutzer gewonnen.
Das Marketing-Team möchte für Q1 2026 einige Features pushen:

1. **Neue Trainingsarten**: Yoga, Schwimmen, HIIT
2. **Export-Formate**: XML und PDF zusätzlich zu CSV/JSON
3. **API für Fitness-Tracker**: Garmin & Fitbit Integration
4. **Premium-Features**: Erweiterte Statistiken und KI-basierte Trainingspläne

Das Dev-Team hat allerdings Bedenken geäußert (siehe Issues im Repository).
Nehmt euch den Issues an und findet eine Lösung für die Bedenken?

Danke & viele Grüße,
Sarah
```
---

## 🎯 Eure Mission

Ihr seid das Entwicklerteam und sollt den Code refactoren, damit:

1. ✅ **Neue Features schnell umsetzbar** sind (Trainingstypen, Export-Formate)
2. ✅ **Tests einfach zu schreiben** sind (ohne komplette DB)
3. ✅ **Code wartbar bleibt** (keine God-Classes, klare Verantwortlichkeiten)
4. ✅ **Production-Bugs vermieden** werden (konsistente Vererbung)
5. ✅ **Team parallel arbeiten** kann (weniger Merge-Konflikte)

**Wichtig:** Die Web-UI ist bereits fertig und funktioniert. Ihr arbeitet **nur am Backend-Code** (Models, Services, Data, Controllers). Die Views müssen **nicht** angefasst werden!

---

## 👥 Team-Organisation

**Setup:** Jeder Entwickler arbeitet an der gleichen Aufgabenstellung.

- Jedes Team bekommt ein **eigenes Repository** von ActiveLog.
- Jedes Team bearbeitet **alle 5 Issues**.
- Jeder Entwickler erstellt sich einen eigenen Branch auf dem er entwickelt.
- Jeder Entwickler versucht eine Lösung zu finden.
- Diskutiert eure Lösungsansätze und helft euch gegenseitig bei Problemen.

---

## 📋 Aufgaben (für euer Team - 5 Issues)

Euer Team (5 Entwickler) soll **alle 5 Issues** beheben. Verteilt die Aufgaben intern:

---

## ✅ Definition of Done (für euer Team)

Eure Team-Lösung ist fertig, wenn:

- [ ] **Alle 5 Issues** wurden bearbeitet
- [ ] Alle Unit-Tests sind grün (`dotnet test`)
- [ ] Die Web-Anwendung startet ohne Fehler (`dotnet run`)
- [ ] Alle Features funktionieren wie vorher (manuell testen!)
- [ ] Euer Code ist verständlich (sprechende Namen, Kommentare wo nötig)
- [ ] Die Views wurden NICHT verändert (nur Backend!)
- [ ] Code-Review im Team gemacht
- [ ] Alle Änderungen sind in euer Team-Repository committed
- [ ] OPTIONAL: Ihr habt neue Unit-Tests für eure Änderungen geschrieben

---

## 🚀 So startet ihr

### 1. Repository-Setup (pro Team)

**Klont euch das Repository lokal**
```bash
# Team-Lead erstellt Fork von ActiveLog auf GitHub
# Alle Team-Mitglieder clonen den Team-Fork
git clone https://github.com/...
cd ActiveLog
```

**Develop Branch erstellen**
```bash
# Jedes Entwickler bekommt einen eigenen develop Branch
git checkout -b develop/<myname>
git push origin develop/<myname>
```

### 2. Dependencies installieren & Testen

```bash
dotnet restore
dotnet build
dotnet test          # Sollte grün sein (16 Tests)
```

### 3. Anwendung ausprobieren

```bash
dotnet run --project=ActiveLog.Web
# → http://localhost:5062
```

Testet alle Features:
- Training erstellen (Cardio, Kraft, Team, Flexibilität)
- Statistiken ansehen
- Export (CSV, JSON)
- Training löschen

**Parallel arbeiten:**
- Jeder Entwickler arbeitet in seinem develop-Branch
- Regelmäßig committen: `git commit -m "Progress on #XX: ..."`
- Regelmäßig pushen: `git push origin develop/<myname>`


**⚠️ Wichtig:**
- Kommuniziert im Team wer gerade was ändert!
- Bei Merge-Konflikten: Zusammen lösen, nicht raten!
- Nach jedem Merge: Tests laufen lassen (`dotnet test`)

### 5. Testen & Abgabe

```bash
# Finale Validierung
dotnet build
dotnet test
dotnet run --project ActiveLog.Web

# Alle Features manuell testen
# Code-Review im Team
```

---

## Hilfreiche Ressourcen

- **SOLID Principles:** [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#solid)
- **Dependency Injection:** [ASP.NET Core DI](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- **Design Patterns:** [Refactoring Guru](https://refactoring.guru/design-patterns)
---

## Während und am Ende der Entwicklung: Team-Präsentation & Vergleich

1. **Präsentation**
   - Zeigt eure Lösung
   - Welche Patterns habt ihr genutzt?
   - Was war schwierig?
   - Was habt ihr gelernt?

2. **Code-Vergleich**
   - Verschiedene Teams = verschiedene Lösungen
   - Welche Ansätze sind besser/schlechter und warum?
   - Diskussion über Trade-offs

3. **Lessons Learned**
   - Was würdet ihr beim nächsten Mal anders machen?
   - Welche SOLID-Prinzipien waren am wichtigsten?
   - Welche Merge-Konflikte hattet ihr?

**Ziel:** Es gibt nicht "die eine richtige Lösung" - aber manche sind wartbarer als andere!

---

**Viel Erfolg! Bei Fragen meldet euch im Team-Chat oder bei der Lehrkraft.** 🚀

---
