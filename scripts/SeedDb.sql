insert into todo_manager.user (username, passwordhash, salt) values ('blaaah', 'J7mC3zApfazfp69FuKwreXPeQfoQbxZ4I/TSxE/5kNw=', '1YYTrcO1x/tsJNqkYvhy8A==') RETURNING *;
insert into todo_manager.user (username, passwordhash, salt) values ('a', 'J7mC3zApfazfp69FuKwreXPeQfoQbxZ4I/TSxE/5kNw=', '1YYTrcO1x/tsJNqkYvhy8A==') RETURNING *;

insert into todo_manager.tag (name, userid) values ('home', 1), ('work', 1);

insert into todo_manager.todo (title, description, priority, duedate, userid) values ('clean', 'clean the house',2, now() + interval '1 week', 1);
insert into todo_manager.todo (title, description, duedate, userid) values ('work', 'finish the project', now() + interval '1 week', 1);
insert into todo_manager.todo (title, description, duedate, userid) values ('work', 'finish the project', now() + interval '1 week', 1);

insert into todo_manager.todo_tag (todoid, tagid) values (1, 1), (2, 2), (3, 2);