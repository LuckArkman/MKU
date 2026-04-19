# Auditoria Técnica Profunda: Projeto MKU (Cursed Stone)

Este documento apresenta uma análise técnica exaustiva, elaborada e densa da arquitetura, sistemas e infraestrutura do projeto **MKU (Cursed Stone)** desenvolvido em Unity.

## 1. Arquitetura de Software e Padrões de Design

O projeto MKU utiliza uma arquitetura híbrida que prioriza a centralização de serviços para facilitar o desenvolvimento rápido em um ecossistema de RPG persistente.

### 1.1 Service Locator e Singleton Global
A classe [Singleton](file:///d:/MKU/My%20project/Assets/MKU/Scripts/Singletons/Singleton.cs) atua como um **Service Locator** central. Ela é o ponto de convergência de todos os sistemas majoritários:
- **Gestão de Dependências**: Centraliza referências a `CharController`, `Inventory`, `QuestManager`, `Market`, entre outros.
- **Persistência de Estado**: Configurada com `DontDestroyOnLoad`, garantindo que o estado global não seja perdido durante transições de cena.

### 1.2 Concorrência e Execução Assíncrona
Uma das peças de engenharia mais refinadas é a [ExecutionQueue](file:///d:/MKU/My%20project/Assets/MKU/Scripts/Strucs/ExecutionQueue.cs):
- **ConcurrentQueue**: Utiliza uma fila de funções de tarefa (`Func<Task>`) para gerenciar a execução sequencial de operações assíncronas.
- **TaskCompletionSource**: Permite que métodos `Run<T>` aguardem resultados de lambdas enfileiradas, garantindo integridade de dados em operações críticas (como transações financeiras ou de inventário).

### 1.3 Padrão DAO (Data Access Object)
Observado em [DialogueDao](file:///d:/MKU/My%20project/Assets/MKU/Scripts/DialogueSistem/DialogueDao.cs), este padrão isola a lógica de persistência e recuperação de dados da lógica de negócios, facilitando a integração com APIs externas.

---

## 2. Ecossistema Web3 e Integração de Backend

O projeto MKU destaca-se pela sua profunda integração com serviços de backend e tecnologias Web3.

### 2.1 Protocolo de Comunicação ([Endpoints.cs](file:///d:/MKU/My%20project/Assets/MKU/Scripts/Strucs/Endpoints.cs))
- **Abstração de Ambientes**: Separa URLs para `UNITY_EDITOR`, `DEBUG` (Heroku Dev) e Produção (Heroku/Vercel).
- **Integração NFT/Moralis**: Possui endpoints específicos para consulta de NFTs via Moralis (`getByAddress`), indicando uma economia baseada em ativos digitais.
- **Wrappers de Rede**: Implementa métodos assíncronos (`SendDataNewAPI`, `GetDataNewAPI`) que envolvem `UnityWebRequest`, com tratamento robusto de códigos de resposta (200-299) e formatação de JSON.

### 2.2 Protocolo de Mensagens (Enums)
O sistema de comunicação é fortemente tipado através de:
- **[RequestCode](file:///d:/MKU/My%20project/Assets/MKU/Scripts/Enums/RequestCode.cs)**: Categoriza a natureza da requisição (User, Lobby, Server, Game, MarketPlace).
- **[ActionCode](file:///d:/MKU/My%20project/Assets/MKU/Scripts/Enums/ActionCode.cs)**: Identifica a ação específica (Login, Equip, Talk, CollectXP, RegisterWalletAddress).

---

## 3. Mecânicas de Jogo e Sistemas Core

### 3.1 Movimentação e Física de Personagem
O [CharController](file:///d:/MKU/My%20project/Assets/MKU/Scripts/CharacterSystem/CharController.cs) implementa uma movimentação de terceira pessoa premium:
- **Matemática Parabólica**: Utiliza `Mathf.Atan2` para calcular ângulos de movimento e suavização via `Mathf.MoveTowardsAngle`, resultando em uma curva de giro natural (`turnCurveSpeed`).
- **Realismo Visual**: Inclui "Bobbing" senoidal (`bobbingFrequency`/`bobbingAmplitude`) para simular movimento de respiração ou balanço do corpo.
- **Independência de Gravidade**: Utiliza um `GravityDAO` customizado para aplicar forças G manualmente, permitindo maior controle em terrenos complexos e terrenos de `Terrain`.

### 3.2 Inventário e Economia
O sistema de [Inventory](file:///d:/MKU/My%20project/Assets/MKU/Scripts/IventorySystem/Inventory.cs) é altamente modular:
- **Slot Isolation**: A herança de `_Slot` e o uso de `ScriptableObjects` para itens permitem a criação de uma vasta gama de colecionáveis.
- **Avaliação de Predicados**: Implementa `IPredicateEvaluator`, permitindo que o sistema de diálogo e quests verifique condições de itens de forma desacoplada.
- **Loop de Crafting**: Integrado via `CraftingManager`, `CookingManager` e `BlackSmithSystem`, suportando receitas (`Recipe.cs`) e ingredientes complexos.

### 3.3 Narrativa e IA
- **Grafos de Diálogo**: Utiliza `DialogueNode` para criar árvores de conversação complexas.
- **ClientAgent**: Gerencia ações desencadeadas por diálogos, como o spawn de monstros (`OnSpawnMonsters`) ou a entrega de quests (`AdduestInCharacter`).
- **Sistema de Quests**: Suporta `QuestChain` (missões encadeadas) e diversos tipos de objetivos (`TaskCondition.cs`, `TaskType.cs`).

---

## 4. Análise de Infraestrutura e Ativos

### 4.1 Organização de Diretórios
- **`Assets/MKU/Scripts`**: Estrutura granular dividida em sistemas (AISystem, QuestSystem, etc.), o que facilita a escalabilidade.
- **`Assets/Art`**: Organizado por tipos de ativos (FX, Fog, Materials, Meshes, UI, URPHDRI). O uso de `ShaderGraph` para grama (`Test_Grass`) indica o uso do **Universal Render Pipeline (URP)**.
- **`Assets/Prefabs`**: Centraliza objetos complexos como `AdaptiveProbeVolumes` e `RTSCameraController`.

### 4.2 Dependências de Pacotes ([manifest.json](file:///d:/MKU/My%20project/Packages/manifest.json))
O projeto utiliza uma stack moderna:
- **Input System (1.17.0)**: Migrado do sistema legado para o novo `Action-based Input System`.
- **Cinemachine (3.1.0)**: Para câmeras dinâmicas e transições suaves.
- **Universal RP (17.3.0)**: Motor de renderização de alta performance.
- **Unity AI Navigation (2.0.9)**: Para pathfinding de NPCs e IA.

---

## 5. Avaliação de Maturidade e Qualidade Técnica

> [!IMPORTANT]
> **Pontos Fortes**: Arquitetura desacoplada em sistemas core, integração robusta com backend assíncrono e uso agressivo de tipagem (Enums) para protocolos de comunicação.

> [!WARNING]
> **Débito Técnico Observado**:
> - **Centralização Excessiva**: O `Singleton` global pode se tornar um gargalo de manutenção.
> - **Inconsistências de Nomenclatura**: Presença de erros tipográficos em namespaces (`DialogueSistem`, `IventorySystem`, `HelthSystem`), que embora não afetem a execução, reduzem a legibilidade profissional.
> - **Complexidade de JSON**: A classe `Endpoints.cs` mistura lógica de UI e rede. Seria recomendado separar as responsabilidades de formatação de JSON para uma camada de `Serializer`.

### Resumo Estatístico Estrutural
- **Diretórios de Scripts**: 32 subfolders especializados.
- **Codificação de Ações**: 34+ códigos de ação definidos para sincronização cliente-servidor.
- **Tipos de Dados**: 70+ definições de enums para controle de estado.

---
*Análise gerada por Antigravity AI em 18 de Abril de 2026.*
