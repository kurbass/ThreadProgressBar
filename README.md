Не большой WinForms проект сделанный за 6 часов с использованием Thread и Semaphore. Я слишком был доволен результатом по этой причине решил сохранить его в github.
ProgressBar создается в месте нажития курсора, его максимальное значение выбирается на угад от 1 до числа в UpDownNumeric а рядом с ProgressBar создается Label который показывает максимальное значение.
Количество одновременно работаующих потоков с использование Semaphore ограничено до 3. 
По нажатию на единственную кнопку в проекте каждый ProgressBar запускается в отдельном потоке и каждую секунду увеличивается на 1, когда ProgressBar заканчивает свою работу он и его Label удаляются и освобождается место для следующего потока.
