require 'albacore'
require 'nuget_helper'

task :default => [:build]
dir = File.dirname(__FILE__)

desc "Rebuild solution"
build :build do |msb, args|
  msb.prop :configuration, :Debug
  msb.target = [:Rebuild]
  msb.sln = "csvdb-studies.sln"
end

desc "Install missing NuGet packages."
nugets_restore :restore do |p|
  p.out = "packages"
  p.nuget_gem_exe
end

desc "test using console"
test_runner :test => [:build] do |runner|
  runner.exe = NugetHelper::nunit_path
  files = Dir.glob(File.join(dir, "**", "bin", "Debug", "Tests.dll"))
  runner.files = files 
end

