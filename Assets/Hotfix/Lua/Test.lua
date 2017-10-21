require 'ExtendGlobal'

base_type = class()
function base_type:ctor(...)
    print 'base ctor'
    self.x = ...
end

function base_type:print_X()
    print(self.x)
end

test1 = class(base_type)
function test1:ctor(...)
    print 'test1 ctor'
    self.w = ...
end

test2 = class(base_type)
function test2:ctor(...)
    print 'test2 ctor'
    self.w = ...
end

c = base_type.new(3)
c:print_X()

a = test1.new(1)
print (a.x)
print (a.w)

b = test2.new(2)
print (b.x)
print (b.w)

a:print_X()
b:print_X()
c:print_X()

