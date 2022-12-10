using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Localization : MonoBehaviour
{
    static private bool isLocalizationLoaded = false;
    static private Dictionary<string, string[]> locDictionary;

    private static void Load()
    {
        locDictionary = new Dictionary<string, string[]> {
            {"Bonus1", new String[]{"Soft kisses", "Мягкие поцелуи", "Weiche Küsse", "Soft Kisses.", "Besos suaves", "Baisers doux", "ソフトキス", "Beijos macios", "柔软的吻" } },
            {"Bonus2", new String[]{"Teasing", "Поддразнивания", "Neckisch", "Dispettoso", "Broma", "Taquinerie", "からかい", "Provocando", "戏弄" } },
            {"Bonus3", new String[]{ "Neck kisses", "Поцелуи в шею", "Nackenküsse", "Baci sul collo", "Besos en el cuello", "Baisers de cou", "首のキス", "Beijos no pescoço", "脖子亲吻" } },
            {"Bonus4", new String[]{ "Petting", "Ласки", "Petting", "Petting", "Caricias", "Caresse", "ペッティング", "Petting.", "抚摸" } },
            {"Bonus5", new String[]{ "Impudent kisses", "Наглые поцелуи", "Unehrliche Küsse", "Baci impudenti", "Besos impudentes", "Bisous impudents", "怒っているキス", "Beijos impudentes", "无礼的吻" } },
            {"Bonus6", new String[]{ "Tits stroking", "Поглаживание грудей", "Titten streicheln.", "Tits locking.", "Tetas de acariciamiento", "Tits caressant", "ティッツストローク", "Tits acariciando", "抚摸" } },
            {"Bonus7", new String[]{ "Booty touching", "Трогание задницы", "Beute-Berührung", "Booty Touching.", "Botín tocando", "Butin touchant", "戦利品に触れる", "Booty tocando", "赃物触摸" } },
            {"Bonus8", new String[]{ "Pussy stroking", "Поглаживание киски", "Muschi streicheln.", "Figa che accarezza", "Acariciamiento de coño", "Caressant la chatte", "オマンコクローク", "Buceta acariciando", "猫抚摸" } },
            {"Bonus9", new String[]{ "Clit masturbation", "Мастурбация клитора", "Clit-Masturbation", "Clit Masturbation.", "Clítoris masturbación", "Masturbation clitoris", "クリットオナニー", "Clitóris masturbação.", "阴蒂手淫" } },
            {"Bonus10", new String[]{ "Gentle sex", "Нежный секс", "Sanfter Sex.", "Sesso delicato", "Sexo suave", "Sexe doux", "穏やかなセックス", "Sexo gentil", "温柔的性别" } },
            {"Bonus11", new String[]{ "Anal sex", "Анальный секс", "Analsex.", "Sesso anale", "Sexo anal", "Sexe anal", "アナルセックス", "Sexo anal", "肛交" } },
            {"Bonus12", new String[]{ "Rough sex", "Грубый секс", "Grobes Geschlecht", "Sesso ruvido", "Sexo duro", "Sexe rugueux", "乱暴なセックス", "Sexo violento", "粗糙的性别" } },

            {"Level1", new String[]{ "Peasant girl", "Крестьянка", "Bauernmädchen", "Ragazza contadina", "Campesina", "Paysan", "田舎娘", "Menina camponesa.", "农民女孩" } },
            {"Level2", new String[]{ "Stock breeder", "Заводчица", "Lagerzüchter", "Allevatore di riserva", "Criador", "Éleveur", "在庫育種", "Criador de ações", "股票饲养员" } },
            {"Level3", new String[]{ "Knight", "Рыцарь", "Ritter", "Cavaliere", "Caballero", "Chevalier", "騎士", "Cavaleiro", "骑士" } },
            {"Level4", new String[]{ "Spider", "Паук", "Spinne", "Ragno", "Araña", "Araignée", "クモ", "Aranha", "蜘蛛" } },
            {"Level5", new String[]{ "Huntress", "Охотница", "Jägerin", "Cacciatrice", "Cazadora", "Chasseure", "ハントレス", "Caçadora", "亨舍斯" } },
            {"Level6", new String[]{ "Noble woman", "Благородная", "Edle Frau", "Donna nobile", "Mujer noble", "Femme noble", "貴族の女性", "Mulher nobre.", "贵族女人" } },
            {"Level7", new String[]{ "Scout", "Разведчица", "Erkunden", "Scout", "Explorar", "Scout", "スカウト", "Scout.", "侦察" } },
            {"Level8", new String[]{ "Magician", "Маг", "Zauberer", "Mago", "Mago", "Magicien", "魔術師", "Mágico", "魔术师" } },
            {"Level9", new String[]{ "Priestess", "Жрица", "Priesterin", "Sacerdotessa", "Sacerdotisa", "Prêtresse", "屋外", "Sacerdotisa", "户外活动" } },
            {"Level10", new String[]{ "Pussy", "Киска", "Muschi", "Figa", "Coño", "Chatte", "プッシー", "Bichano", "猫"} },

            {"UI_Bonuses", new String[]{ "Bonuses!", "Бонусы!", "Boni!", "Bonus!", "Bonos!", "Bonus!", "ボーナス！", "Bônus!", "奖金！" } },
            {"UI_Level", new String[]{ "Level ", "Уровень", "Niveau", "Livello", "Nivel", "Niveau", "レベル", "Nível", "等级" } },
            {"UI_ProgressMode", new String[]{"Progress Mode","Режим прогресса","Progress-Modus","Modalità Progress","Modo De Progreso","Mode De Progrès","プログレスモード","Modo Progress","进度模式"} },
            {"UI_FreeMode", new String[]{ "Free Mode", "Свободный режим", "Freier Modus", "Modalità libera", "Modo Libre", "Mode Libre", "フリーモード", "Modo Livre", "自由模式" } },
            {"UI_ClicksSlash", new String[]{ "/Click:", "/Клик:", "/Klicken:", "/Clic:", "/Hacer Clic:", "/Cliquez Sur:", "/クリック：", "/Clique:", "/点击：" } },
            {"UI_Sec", new String[]{ "/Sec:", "/Секунду:", "/Sec:", "/Sec:", "/Segundo:", "/Seconde:", "/秒：", "/Sec:", "/秒：" } },
            {"UI_10Sec", new String[]{ "/10Sec:", "/10 Секунд:", "/10Sec:", "/10Sec:", "/10seg:", "/10sec:", "/10秒：", "/10 Seg:", "/10秒：" } },
            {"UI_Loading", new String[]{ "Loading...", "Загрузка...", "Wird geladen...", "Caricamento in corso...", "Cargando...", "Chargement...", "読み込んでいます...", "Carregando...", "载入中..." } },
            {"UI_ClickMe", new String[]{ "Click me!", "Нажми на меня!", "Klick mich!", "Cliccami!", "¡Haz Click En Mi!", "Moi!", "私をクリックしてください！", "Clique Me!", "点击我！" } },
            {"UI_HearTlick", new String[]{ " ♥/Click", " ♥/Клик", " ♥/Click", "♥/clic", "♥/Pulse", "♥/Cliquez", "♥/クリック", "♥/Click", "♥/点击" } },
            {"UI_Heart10Sec", new String[]{ " ♥/10 Sec", " ♥/10 Сек", " ♥/10 Sec", "♥/10 Sec", "♥/10 Sec", "♥/10 Sec", "♥/10秒", "♥/10 Sec", "♥/10秒" } },
            {"UI_HeartSec", new String[]{ " ♥/Sec", " ♥/Сек", " ♥/Sec", "♥/Sec", "♥/Sec", "♥/Sec", "♥/秒", "♥/Sec", "♥/秒" } },
            {"UI_UnlockByBuying", new String[]{"Unlock by buying previous bonus!","Открой купив предыдущий бонус","Entsperren von vorherigen Bonus zu kaufen!","Sbloccare con l'acquisto di bonus precedente!","Desbloqueo mediante la compra de bonos anterior!","Débloquer en achetant précédent bonus!","前回のボーナスを購入することでロックを解除！","Desbloquear comprando bônus anterior!","解锁通过购买之前的奖金！"} },
            {"UI_LevelComplete", new String[]{ "LEVEL COMPLETE", "УРОВЕНЬ ПРОЙДЕН", "LEVEL GESCHAFFT", "LIVELLO COMPLETATO", "NIVEL COMPLETADO", "NIVEAU COMPLET", "レベル完了", "NÍVEL COMPLETO", "水平完成" } },
            {"UI_DLCWait", new String[]{"DLC: wait for the next update!","DLC: жди следующего апдейта!","DLC: Warten auf das nächste Update!","DLC: attesa per il prossimo aggiornamento!","DLC: espera para la próxima actualización!","DLC: attendre la prochaine mise à jour!","DLC：次回の更新を待ちます！","DLC: espera para a próxima atualização!","DLC：等待下一次更新！"} },
            {"UI_LoadingGallery", new String[]{"Loading...","Загрузка...","Wird geladen...","Caricamento in corso...","Cargando...","Chargement...","読み込んでいます...","Carregando...","载入中..." } },
            {"UI_GalleryCaption", new String[]{ "Gallery", "Галерея", "Galerie", "Galleria", "Galería", "Galerie", "画廊", "Galeria", "画廊" } },
            {"UI_ArtBy", new String[]{ "Art by KnightFang", "Арт от KnightFang", "Kunst von KnightFang", "Stampe di KnightFang", "Arte por KnightFang", "Art par KnightFang", "KnightFangアート", "Arte KnightFang", "艺术的KnightFang" } },
            {"UI_Play", new String[]{ "PLAY", "ИГРА", "ABSPIELEN", "GIOCARE", "TOCAR", "JOUER", "演奏する", "REPRODUZIR", "玩" } },
            {"UI_SettingsButton", new String[]{ "SETTINGS", "НАСТРОЙКИ", "DIE EINSTELLUNGEN", "IMPOSTAZIONI", "AJUSTES", "LES PARAMÈTRES", "設定", "DEFINIÇÕES", "设置" } },
            {"UI_DLC", new String[]{ "DLC", "DLC", "DLC", "DLC", "DLC", "DLC", "ダウンロードコンテンツ", "DLC", "DLC" } },
            {"UI_GalleryButton", new String[]{ "GALLERY", "ГАЛЕРЕЯ", "GALERIE", "GALLERIA", "GALERÍA", "GALERIE", "ギャラリー", "GALERIA", "画廊" } },
            {"UI_StatsButton", new String[]{ "STATS", "СТАТИСТИКА", "STATISTIKEN", "STATISTICHE", "STATS", "STATISTIQUES", "STATS", "ESTATÍSTICAS", "统计" } },
            {"UI_ProgressRecommended", new String[]{ "Progress Mode (recommended)", "Режим прогресса (рекомендуется)","Progressiv-Modus (reccomended)","Modalità Progress (CONSIGLIATI)","Modo de progreso (recomendados)","Mode de progrès (reccomended)","プログレスモード（reccomended）","Modo de progresso (reccomended)","进度模式（受到推崇的）"} },
            {"UI_FreeModeDLC", new String[]{"Free Mode!","Свободный режим: новые девушки в следующих DLC!","Free-Modus: neue Mädchen im freien DLC incoming!","Free Mode: in arrivo nuove ragazze in DLC gratis!","Modo libre: nuevos entrantes en niñas DLC gratis!","Mode libre: de nouvelles filles entrant en DLC gratuit!","フリーモード：無料DLCに入ってくる新しい女の子！","Modo livre: meninas novas recebidas no DLC grátis!","自由模式：新的女孩免费DLC来袭！"} },
            {"UI_SettingsCaption", new String[]{ "Settings", "Настройки", "Die Einstellungen", "Impostazioni", "Ajustes", "Paramètres", "設定", "Definições", "设置" } },
            {"UI_Language", new String[]{ "Language", "Язык", "Sprache", "Linguaggio", "Idioma", "Langue", "言語", "Língua", "语" } },
            {"UI_Music", new String[]{ "Music", "Музыка", "Musik", "Musica", "Música", "Musique", "音楽", "Música", "音乐" } },
            {"UI_Sound", new String[]{ "Sound", "Звук", "Klang", "Suono", "Sonar", "Sonner", "音", "Som", "声音" } },
            {"UI_ClearProgress", new String[]{ "Clear Progress", "Очистить прогресс", "Deutliche Fortschritte", "Chiaro Progress", "claro Progreso", "progrès Effacer", "クリア進捗状況", "Limpar Progress", "明显的进步" } },
            {"UI_ReallyWant", new String[]{"Do you REALLY want to erase all save data?","Вы ДЕЙСТВИТЕЛЬНО хотите удалить все данные сохранений?","Wollen Sie wirklich alle Daten speichern löschen?","Non si ha realmente desidera cancellare tutti i dati di salvataggio?","¿Realmente desea borrar todos los datos guardados?","Voulez-vous vraiment supprimer toutes les données de sauvegarde?","あなたは本当にセーブデータすべてを消去しますか？","Você realmente quer apagar todos os dados guardados?","你真的要删除所有保存的数据？"} },
            {"UI_Delete", new String[]{ "DELETE", "УДАЛИТЬ", "LÖSCHEN", "ELIMINA", "ELIMINAR", "EFFACER", "削除", "EXCLUIR", "删除" } },
            {"UI_Cancel", new String[]{ "Cancel", "Отмена", "Stornieren", "Annulla", "Cancelar", "Annuler", "キャンセル", "Cancelar", "取消" } },
            {"UI_StatsCaption", new String[]{ "Stats", "Статистика", "Statistiken", "Statistiche", "Estadísticas", "Statistiques", "統計", "Estatísticas", "统计" } },
            {"UI_Clicks", new String[]{ "Clicks", "Клики", "Klicks", "Clic", "Clics", "Clics", "クリック数", "Cliques", "点击" } },
            {"UI_HeartsAcquired", new String[]{ "Hearts Acquired", "Сердец собрано", "Herz-Acquired", "Cuori acquisita", "Corazones Adquirida", "Coeurs Acquise", "ハーツを買収", "Corações Adquirida", "心收购" } },
            {"UI_HeartsAcquiredClicks", new String[]{"Hearts Acquired by Clicks","Сердец собрано кликами","Hearts von Klicks Erworbene","Cuori Acquisita da Clic","Corazones Adquirida Por Clics","Coeurs Acquis Par Clics","ハーツクリック数によって獲得","Corações Adquirida Pela Cliques","心的点击次数收购"} },
            {"UI_HeartsAcquiredSec", new String[]{"Hearts Acquired by Time Bonuses (sec)","Сердец собрано ежесекундными бонусами","Herzen Erworben von Zeitgutschriften (s)","Cuori Acquisita da bonus di tempo (sec)","Corazones Adquiridos Por Bonos De Tiempo (Seg)","Coeurs Acquis Par Bonus Temps (S)","タイムボーナスが買収ハーツ（秒）","Corações Adquirida Pela Bónus Tempo (Seg)","通过时间奖金后天心（秒）"} },
            {"UI_HeartsAcquired10Sec", new String[]{"Hearts Acquired by Time Bonuses (10 sec)","Сердец собрано бонусами в 10 секунд","Herzen Erworben von Zeit Boni (10 sec)","Cuori Acquisita da bonus di tempo (10 sec)","Corazones Adquiridos Por Bonos De Tiempo (10 Segundos)","Coeurs Acquis Par Bonus Temps (10 Sec)","タイムボーナスが買収ハーツ（10秒）","Corações Adquirida Pela Bónus De Tempo (10 Seg)","按时间奖金后天心（10秒）"} },
            {"UI_HeartsSpent", new String[]{ "Hearts Spent", "Сердец потрачено", "Herzen Spent", "Cuori trascorso", "Pasamos Corazones", "Coeurs Dépensés", "使用済みハーツ", "Corações Passado", "心花" } },
            {"UI_MaxLevel", new String[]{ "Max Level", "Максимальный уровень", "Maximales Level", "Livello Max", "Máximo Nivel", "Niveau Maximum", "最大レベル", "Nível Máximo", "最高等级" } },
            {"UI_BonusesBought", new String[]{"Bonuses Bought","Бонусов куплено","Boni Gekauft","Bonus comprato","Bonificaciones Compraron","Bonus Acheté","買ったボー​​ナス","Bônus Comprou","奖金买"} },
            {"UI_FULL", new String[]{"FULL","ВСЁ","VOLL","COMPLETO","COMPLETO","PLEIN","フル","CHEIO","满的"} },
            {"UI_Thanks", new String[]{
                "You won! Thanks for completing the main part of the game. Please give honest feedback to the game - this will greatly support the indie developer.",
                "Вы победили! Спасибо, что осилили основную часть игры! Пожалуйста, оставьте игре честный отзыв - это очень поможет независимым разработчикам!",
                "Du hast gewonnen! Danke, dass Sie den Hauptteil des Spiels abgeschlossen haben. Bitte geben Sie dem Spiel ehrliches Feedback an - dies wird den Indie-Entwickler erheblich unterstützen.",
                "Hai vinto! Grazie per aver completato la parte principale del gioco. Si prega di dare un feedback onesto al gioco - questo supporterà notevolmente lo sviluppatore indie.",
                "¡Ganaste! Gracias por completar la parte principal del juego. Por favor, dé un comentario honesto al juego, esto apoyará enormemente al desarrollador indie.",
                "Tu as gagné! Merci d'avoir terminé la partie principale du jeu. S'il vous plaît donner des commentaires honnêtes au jeu - cela appuiera grandement le développeur indépendant.",
                "あなたは勝ちました！ゲームの主要部分を完了してくれてありがとう。ゲームに正直なフィードバックをお願いします - これはIndie開発者を大幅にサポートします。",
                "Você ganhou! Obrigado por completar a parte principal do jogo. Por favor, dê feedback honesto para o jogo - isso apoiará muito o desenvolvedor Indie.",
                "你赢了！感谢您完成游戏的主要部分。请为游戏提供诚实的反馈 - 这将极大地支持独立开发人员。"
            } },
            {"UI_Ok", new String[]{"OK","OK","OK","OK","OK","OK","OK","OK","好的"} },
            {"UI_YouWin", new String[]{ "YOU WIN", "ПОБЕДА", "DU GEWINNST", "HAI VINTO", "TÚ GANAS", "VOUS GAGNEZ", "あなたが勝ちます", "VOCÊ GANHA", "你赢了"  } },
            {"UI_Move", new String[]{ "Move cursor", "Движение курсора", "Cursor bewegen", "Sposta il cursore", "Mover cursor", "Déplacer le curseur", "カーソルを移動", "Mover o cursor", "移动光标" } },
            {"UI_Click", new String[]{ "Activate", "Нажатие", "Aktivierung", "Attivazione", "Activación", "Activation", "活性化", "Ativar", "启用" } }
        };
    }

    static public string l(string key)
    {
        if (!isLocalizationLoaded)
        {
            isLocalizationLoaded = true;
            Load();
        }
        int language = getLanguageIndex();
        return locDictionary[key][language];
    }

    private static int getLanguageIndex()
    {
        string languageString = SteamWrapper.GetCurrentGameLanguage().ToLower();
        if(languageString == "russian")
        {
            return 1;
        }
        if (languageString == "german")
        {
            return 2;
        }
        if (languageString == "italian")
        {
            return 3;
        }
        if (languageString == "spanish")
        {
            return 4;
        }
        if (languageString == "french")
        {
            return 5;
        }
        if (languageString == "japanese")
        {
            return 6;
        }
        if (languageString == "portuguese")
        {
            return 7;
        }
        if (languageString == "schinese")
        {
            return 8;
        }
        return 0;
    }
}
    /*public static System.Object Load()
    {
        string fileName = Application.dataPath + "/Resources/loc.json";
        if (File.Exists(fileName))
        {
            String dataAsJson = File.ReadAllText(fileName);
            Debug.Log(dataAsJson);
            System.Object data = JsonUtility.FromJson(dataAsJson, Dictionary<String,String>);

            return dataAsJson;
        }
        else
        {
            Debug.Log("no file " + fileName);
            return null;
        }
    }*/
