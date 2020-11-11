from LinkedList import Linked_list
from validation import Validation as v
from classContext import Context
from strategyIterator import StrategyIterator
from strategyFile import StrategyReadFile
from Observer import Observer
from Logger import Logger
from Event import Event
import copy
import threading
from threading import Thread

def main():
    Observer.attach('add', Logger.log)
    Observer.attach('delete', Logger.log)
    Observer.attach('deleteInRange', Logger.log)
    Observer.attach('taskMethod', Logger.log)

    linked_list1 = Linked_list()
    linked_list2 = Linked_list()
    options = '1 - first list \n2 - second list\n3 - list method\n4 - print lists\n5 - exit\n'
    while True:
        try:
            print(options)
            choice = v.intValidateInRange('Enter choice ', 1, 5)

            if choice == 1: linked_list1 = generateList(linked_list1)
            elif choice == 2: linked_list2 = generateList(linked_list2)
            elif choice == 3: linked_list1, linked_list2 = listMethod(linked_list1, linked_list2)
            elif choice == 4: print('list 1 - {}\nlist 2 - {}\n'.format(linked_list1, linked_list2))
            elif choice == 5: break

        except Exception as e:
                print('Error ', '--'*15, '  ',e)


def generateList(linked_list: Linked_list):
    options = ' 1 - Strategy 1\n 2 - Strategy 2\n 3 - generate data\n 4 - print list\n 5 - exit\n'
    context = Context(StrategyIterator)
    while True:
        try:
            print(options)
            choice = v.intValidateInRange('Enter choice ', 1, 5)

            if choice == 1: context.strategy = StrategyIterator()  
            elif choice == 2: context.strategy = StrategyReadFile()
            elif choice == 3: linked_list = generateMenu(linked_list, context)
            elif choice == 4: print(linked_list)
            elif choice == 5: return linked_list

        except Exception as e:
            print('Error ', '--'*15, '  ',e)

def generateMenu(linked_list: Linked_list, context):
    position = v.intValidateInRange('Enter position ', 0, len(linked_list))
    linked_list = context.do_some_business_logic(linked_list, position)
    return linked_list

class ThreadWithReturnValue(Thread):
    def __init__(self, group=None, target=None, name=None,
                 args=(), kwargs={}, Verbose=None):
        Thread.__init__(self, group, target, name, args, kwargs)
        self._return = None
    def run(self):
        print(type(self._target))
        if self._target is not None:
            self._return = self._target(*self._args,
                                                **self._kwargs)
    def join(self, *args):
        Thread.join(self, *args)
        return self._return

def listMethod(linked_list1: Linked_list, linked_list2: Linked_list):
    options = '  1 - remove at\n  2 - remove in range\n  3 - list method\n  4 - print lists\n  5 - exit\n'
    #hread1 = threading.Thread()
    #thread2 = threading
    while True:
        try:
            print(options)
            choice = v.intValidateInRange('Enter choice ', 1, 5)
            if choice == 1:
                #thread1 = threading.Thread(target=deleteAtMenu, args=(linked_list1))
                #thread1.start()
                #thread2 = threading
                #linked_list = deleteAtMenu(linked_list1)
                #thread1.join()
                twrv = ThreadWithReturnValue(target=deleteAtMenu, args=linked_list1)
                twrv.start()
                linked_list1 = twrv.join()
            elif choice == 2:
                linked_list = deleteInRangeMenu(linked_list1)     
            elif choice == 3:
                linked_list = listMethodMenu(linked_list1)
            elif choice == 4:
                print('list 1 - {}\nlist 2 - {}\n'.format(linked_list1, linked_list2))
            elif choice == 5:
                return linked_list1, linked_list2

        except Exception as e:
            print('Error ', '--'*15, '  ',e)

def deleteAtMenu(linked_list: Linked_list):
    beforeList = copy.deepcopy(linked_list)
    position = v.intValidateInRange('Enter position', 0, len(linked_list)-1)
    linked_list.remove_at(position)
    Event.do_some('delete', [beforeList, position, linked_list])
    return linked_list

def deleteInRangeMenu(linked_list: Linked_list):
    beforeList = copy.deepcopy(linked_list)
    print(f"From 0 to {len(linked_list) - 1}")
    l, r = v.int_validate_range()
    linked_list.remove_in_range(l, r)
    Event.do_some('deleteInRange', [beforeList, [l, r], linked_list])
    return linked_list

def listMethodMenu(linked_list: Linked_list):
    beforeList = copy.deepcopy(linked_list)
    l, r = linked_list.min_max(linked_list)
    j = 1
    if r < l:
        temp = l
        l = r
        r = temp
        
    print(f'left index - {l}, \nright index - {r}')
    for i in range((r - l)//2):
        temp = linked_list.get_at(l+j)
        linked_list.set_at(l + j, linked_list.get_at(r - j))
        linked_list.set_at(r - j, temp)
        j += 1
    Event.do_some('taskMethod', [beforeList, [l, r], linked_list])
    return linked_list



if __name__ == '__main__':
    main()

    








