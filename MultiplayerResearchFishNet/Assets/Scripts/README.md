# Multiplayer Workshop 2
*Nick Buisman, Robin van Dommelen, Niels van Kuppevelt, Ivan Teunesen, Jorne Wessels*

## Dodgeball
Het doel voor deze workshop is om met standaard componenten van [Fishnet](https://fish-networking.gitbook.io/docs) te leren werken.

De te maken applicatie dient de volgende onderdelen te bevatten:
* Een wereld met verschillende spawnpunten
* Een FishNet NetworkManager
* Een NPC (non-playable character)
* Een speler-prefab
* Een projectiel wat de speler kan afschieten

## 1. FishNet installeren
Om te beginnen met multiplayer moet eerst het Unity-package van FishNet geinstalleerd worden. Dit kan via deze [link](https://assetstore.unity.com/packages/tools/network/fishnet-networking-evolved-207815).

Om FishNet te gebruiken kunnen de assets (zoals de eerder genoemde NetworkManager) toegevoegd worden aan een standaard Unity-project.

## 2. Het project opstellen
Ga naar de [demo repository op GitHub]() en clone de [*naam*] branch. Open vervolgens het project in Unity.

Voordat spelers kunnen worden ingeladen in de scene, moet de scene van een aantal assets voorzien zijn. Namelijk:
1. Een speelbare scene, dus een plane waarop de spelers kunnen staan
2. Een spawnpunt, waarop de speler kan worden geinstantiëerd.
3. Een FishNet [NetworkManager](https://fish-networking.gitbook.io/docs/manual/guides/components/managers/network-manager) prefab.

### 2a. De scene maken
Binnen het project is een demo-scene genaamd ```naam``` aangeleverd, open deze.

De scene bevat een standaard spelwereld met een aantal objecten waaracher spelers zich kunnen verschuilen en op kunnen springen.

### 2b. Een spawnpunt in de scene toevoegen
Vervolgens moet een speler in de scene geladen kunnen worden. Dit kan worden gedaan door gebruik te maken van *spawnpoints*.

Om een spawnpoint te maken is een transform nodig, zodat de game posities binnen de gamewereld heeft waar de spelers ingeladen kunnen worden. Een leeg GameObject volstaat hiervoor.

Maak binnen het ```World``` GameObject een nieuw leeg GameObject aan met de naam ```Spawnpoint```. Plaats dit GameObject op een willekeurige plek op de ```Ground``` plane.

### 2c. De NetworkManager configureren
De scene is nu bijna klaar. Als laatste stap moet FishNet weten waar de spawnpoints in de scene zich bevinden, zodat er vervolgens spelers op kunnen worden geinstantiëerd.

Met FishNet toegevoegd aan het Unity-project, navigeer naar:
> Assets > FishNet > Demos > Prefabs

Sleep vervolgens de ```NetworkManager```prefab in de scene.

Binnen de prefab staan een aantal velden:
1. NetworkManager
2. ObserverManager
3. PlayerSpawner

Navigeer naar de *PlayerSpawner* en vouw het veld *Spawns* uit. Hier kan de transform van het eerder gemaakte ```Spawnpoint``` in gesleept worden. FishNet herkent dit GameObject nu als een locatie waar *Spawnable Prefabs* kunnen worden geinstantiëerd.

Om deze *Spawnable Prefabs* aan te geven, kunnen we binnen het *NetworkManager* component van de NetworkManager-prefab een Spawnable Prefabs-Asset definiëren. FishNet maakt hiervoor standaard al een asset aan, maar indien gewenst kan een andere collectie ook zelf gecreëerd worden binnen de Asset-directory:

> Create > FishNet > Spawnable Prefabs

Binnen deze asset staat gedefiniëerd welke objecten FishNet kan instantiëren op de aangegeven spawnpoints. Hierover later meer.

Selecteer deze asset als *Spawnable Prefabs* in het *NetworkManager* component van de NetworkManager.

## 3. De speler toevoegen
Nu de scene is opgesteld kan de speler aangemaakt worden, zodat de scene straks speelbaar is.

Er is al een speler-prefab aangeleverd binnen het project, echter ontbreekt het een aantal FishNet-gerelateerde onderdelen. Voor nu kan de speler, in theorie, rondlopen en springen. De speler moet echter ook kunnen schieten, en opnieuw spawnen wanneer deze wordt geraakt door projectielen van andere spelers.

### 3a. De speler laten schieten
Om de speler projectielen te laten schieten kan een apart script gedefinieerd worden, genaamd ```PlayerShootProjectile```. Dit script is van het type *NetworkBehaviour*, afgeleid van de Unity MonoBehaviour-klasse. Dit is de klasse die FishNet gebruikt voor onderdelen die over het netwerk verstuurd worden.

Het begin van deze klasse is al gegeven:
```cs
public class PlayerShootProjectile : NetworkBehaviour
{
    private GameObject projectileSpawner;
    public GameObject projectile;

    public void Start()
    {
        if (projectile is null) throw new Exception("projectile not set!");
        projectileSpawner = transform.Find("Projectile Spawner").gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnProjectile(
                projectile,
                projectileSpawner.transform.position,
                projectileSpawner.transform.rotation
            );
        }
    }
}
```

Deze code moet nu over het netwerk kunnen communiceren met de server/host, om aan te geven dat de speler een projectiel wil schieten. Hiervoor kan gebruik worden gemaakt van de ```[ServerRpc]``` tag. Deze tag staat de client toe om code op de server uit te voeren. Met deze tag kan de functie ```SpawnProjectile``` aangemaakt worden:

```cs
// Any required function parameters have not been defined in this example
[ServerRpc]
private void SpawnProjectile()
{
	//TODO: Implement
}
```

Om het projectiel in te spawnen moet de server weten wat deze moet instantieren, waar dit moet, en in welke richting. Om aan de server aan te geven dat er iets geinstantiëerd moet worden, kan gebruik gemaakt worden van ```ServerManager.Spawn()```.

> ServerManager is een public variable binnen de NetworkBehaviour-klasse, en kan aangeroepen worden binnen implementaties van deze klasse.

Ook moet de client niet voor elke geinstantiëerde speler in de lobby kunnen schieten. Hiervoor maakt FishNet gebruik van een functie genaamd *Ownership*. Als een client geen ownership heeft van objecten in een scene kan het gebruik hiervan geblokkeerd worden.

Om te zorgen dat de client alleen via zijn eigen positie een projectiel kan afschieten kan de volgende functie aan het ```PlayerShootProjectile``` script toegevoegd worden:

```cs
public override void OnStartClient()
{
    base.OnStartClient();
	// If the instance of the script doesn't belong to the client that requested the call, disable the component
    if (!IsOwner) GetComponent<PlayerShootProjectile>().enabled = false;
}
```

### 3b. De speler laten respawnen
Indien de speler geraakt wordt door een andere speler moet deze opnieuw op een ander spawnpunt geinstantiëerd worden. Hiervoor kan het script ```PlayerRespawn``` aan de speler toegevoegd worden:

```cs
	// Add this when it works
```

*NADER TE BEHANDELEN WANNEER HET WERKT*

### 3c. De speler toevoegen aan de scene
Om de speler *spawnable* te maken binnen de scene, moet deze toegevoegd worden aan de NetworkManager.

In het Assets-tabblad van de Project-Directory, selecteer het object ```DefaultPrefabObjects```.
Druk op het + teken onderaan de Prefabs lijst, en voeg hier de Player-prefab aan toe.

Navigeer terug naar de NetworkManager-prefab. Om aan de NetworkManager aan te geven wat de speler is, moet in de *PlayerSpawner* een *Player Prefab* aangegeven worden. Selecteer hiervoor de player-prefab.

## 4 Een NPC toevoegen
Voor het geval dat er geen andere spelers aan de lobby deelnemen wordt er een NPC (*Non-Playable Character*) aan de scene toegevoegd. Ook hiervoor is al een prefab aangeleverd.

Ook een NPC kan worden uitgeschakeld. Om ervoor te zorgen dat deze opnieuw kan spawnen worden ook voor de NPC spawnpoints gebruikt.

Maak een leeg GameObject aan met de naam ```NpcSpawnPoints```. Hierin kunnen NpcSpawnpoint prefabs toegevoegd worden. Deze bevatten een NetworkObject component en een ```NPCSpawnScript``` script:

```cs
public class NpcSpawnScript : NetworkBehaviour
{
    public GameObject npcPrefab;
    public float spawnCooldown = 5f;

    private float spawnTimer;
    private bool npcActive;
    private GameObject npc;

    private void Start()
    {
        if (!IsServerStarted) Destroy(gameObject);
        else ResetSpawner();
    }

    private void Update()
    {
        if (npc) return;
        if (npcActive)
        {
            ResetSpawner();
            return;
        }
        var dt = Time.deltaTime;
        if (spawnTimer > 0) spawnTimer -= dt;
        else SpawnNpc();
    }

    private void SpawnNpc()
    {
        npc = Instantiate(npcPrefab, transform.position, transform.rotation);
        Spawn(npc);
        npcActive = true;
    }

    private void ResetSpawner()
    {
        transform.DestroyChildren();
        npc = null;
        npcActive = false;
        spawnTimer = spawnCooldown;
    }
}
```

> Dit is FishNet-specifieke code, maar de implementatie verschilt te weinig van standaard Unity om langdurig behandeld te worden.

Voeg aan het NPC spawn script de NPC-prefab toe, en voer een cooldown periode naar wens in in seconden.

## 5 Met andere spelers verbinden
Indien alles goed is ingesteld kan de game gedeeld worden met een andere persoon. Als beide personen dezelfde versie en build op het zelfde netwerk opstarten kan vervolgens via de knoppen in de FishNet HUD gekozen worden om een server en een client op te starten.

> De persoon die de server start dient ook om een client te starten. Indien er al een server is op een ander systeem hoeft enkel een client opgestart te worden.