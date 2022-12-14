# Тестовое задание в Brickworks Games

## Ассеты умений игрока
Умения игрока описываются с помощью Scriptable Object'ов (SO)

### Данные
Данные умений поделены на 2 части: логика и визуал

#### Логические данные (класс `PlayerSkillConfig`)
* `Id` — `string` id (одно из существующих в коде в файле `DataFactory_Types`)
* `LearningCost` — `int` стоимость изучения
* `Neighbours` — список `string` id соседних умений игрока

#### Визуальные данные (класс `PlayerSkillViewConfig`)
* `SkillId` — `string` id (одно из существующих в коде в файле `DataFactory_Types`)
* `Name` — `string` отображаемое название
* `Color` — `Color` цвет заливки кружка умения
* `ScreenPosition` — `Vector3` позиция на экране

### Создание ассетов умений
1. В окне `Project` клик правой кл. мышки по пустому месту в папке
2. Выбрать подпункт `Create` 
3. Выбрать `Player Skill`

### Синхронизация данных об умениях
SO умений хранятся в папке `Assets/Player Skills`. Все данные об умениях сериализуются в виде списка конфигов в гейм обжекте `Managers/ConfigsManager` на сцене `Main`. Чтобы изменения в ассетах скиллов применились есть кнопка `Link Skill Configs`, с помощью которой происходит пересохранение всех ассетов скиллов из вышеуказанной папки.

## DataFactory (оправдание)
По моему опыту, зачастую в играх необходимо десереализовать слегка отличающиеся данные одного абстрактного типа. Например:
* Награда
  * Награда опытом (поля: абсолютное значение опыта, процент от текущего опыта игрока)
  * Награда предметом (поля: id предмета, его статы)

Для реализации такой конструкции я использую статический класс, в котором создаётся такая конструкция:
Dictionary<`абстрактный_тип`, Dictionary<`id_конкретного_типа`, `конкретный_тип`>>
(например Dictionary<`AbstractReward`, Dictionary<`gold`, `GoldReward`>>)

Важно отметить, что id сущности и её конфига должны совпадать. Например `kill` и для `KillPlayerSkillConfig`, и для `KillPlayerSkill`.

В дальнейшем в данных указывается id соответствущий типу и соответствущие типу полЯ. При десериализации и конструировании сущностей из конфигов используются типы, найденные в словаре по id.

## Примечания
* Возможность сохранения данных сессий игрока не упомяналась в ТЗ, поэтому не была реализована. Но заглушка под неё есть (см. `DataManager.Init`)
* Позиция умения игрока в поле `ScreenPosition` указывается в неких абстрактных единицах. При импорте она умножается на значение поля `screenPosModifier` на скрипте геймобъекта `Managers/ConfigsManager`. Сделано это для более быстрых итераций (примерно по схеме выставил абстрактные координаты на глаз относительно других объектов и дальше множитель только меняешь)
* Такой воркфлоу (создай/измени ассет умения, потом сериализуй изменения кнопкой на скрипте менеджера) получился в результате желания имитировать подгрузку всех конфигов из какого-то одного места. В данном случае этим местом оказались сериализованные данные скрипта, но это может быть легко изменено на чтение данных из json-файлов или ещё откуда-то. Прописывать конфиги прямо в списке в редакторе мне показалось попросту не удобно, поэтому я выбрал какой-то средний способ между полностью хорошо сделать на SO и сделать подгрузку из какого-то json'а опять же.