mergeInto(LibraryManager.library, {
    DownloadFile: function (filename, content) {
        // Создаём объект Blob с данными из C#
        var blob = new Blob([UTF8ToString(content)], { type: 'text/plain' });

        // Создаём элемент `a` и инициируем скачивание
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = UTF8ToString(filename);

        // Имитируем клик по ссылке
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);

        // Очищаем URL
        URL.revokeObjectURL(a.href);
    },
});
