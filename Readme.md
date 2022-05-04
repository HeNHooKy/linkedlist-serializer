**Custom serializer implementation:**

Please provide the following:

1. Provide the ```IListSerializer``` interface implementation 
(you can use any serialization format but you could not utilize third-party libraries that will serialize the full list for you. I.e. it's allowed to utilize third-party libraries for serializing 1 node in particular format):
- provided data structures, class names or namespaces could not be changed;
- solution is allowed to be not thread safe;
- it's guaranteed that list provided as an argument to ```Serialize``` and ```DeepCopy``` function is consistent and doesn't contain any cycles;
- automated testing of your solution will be performed, the resulting rate for the solution will be given based on (in order of priority):
  - tests on correctness of the solution 
  - performance tests 
  - tests on memory consumption
  
2. Write your own test cases for the implementation for ```IListSerializer``` interface.


**My solution thinking**

1. I need to create at least two solutions: the first is solution with library for serializtion of one node and the second one is a soluction without any library.
2. I need to test what solution will have best perfomance.
3. I can even test my solution of serializing node and third-party solution and choose better one.
4. Now I see two difficult points: the first one is some random ref to list node, and the second one is DeepCopy method
5. When I complete solution I need to write some tests and make memory(how to do it?) and time consumption tests


**Solution**

1. First at all I need to create or find good solution for string(bytes) representetion of node list
2. I need to build some system wich will have very fault tolerance flow
3. Let's pretend that our list node is related list, and previous of next node relates to current node every time if next node isn't null. This supposing is needed to know that our random ref has some index in our seralization stream