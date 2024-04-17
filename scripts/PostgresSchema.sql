drop schema if exists todo_manager cascade;
create schema todo_manager;

create table todo_manager.user (
                                   id serial primary key,
                                   username text not null,
                                   passwordhash text not null,
                                   salt text not null
);

create table todo_manager.todo (
                                   id serial primary key,
                                   title text not null,
                                   priority int not null default 0,
                                   description text,
                                   createdat timestamp not null default (now() at time zone 'utc'),
                                   duedate timestamp not null default (now() at time zone 'utc') + interval '1 week',
                                   iscompleted boolean not null default false,
                                   userid int references todo_manager.user(id)
);

create table todo_manager.tag (
                                  id serial primary key,
                                  name text not null,
                                  userid int references todo_manager.user(id)
);

create table todo_manager.todo_tag (
                                       todoid int references todo_manager.todo(id) on delete cascade,
                                       tagid int references todo_manager.tag(id) on delete cascade ,
                                       primary key (todoid, tagid)
);