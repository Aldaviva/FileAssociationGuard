using System;
using System.ComponentModel.DataAnnotations;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace FileAssociations {

    public static class MainClass {

        private const bool DRY_RUN = false;

        private static Logger? LOGGER;

        public static int Main() {
            initLogging(LogLevel.Trace);

            LOGGER = LogManager.GetCurrentClassLogger();

            var fileAssociationService = new FileAssociationService(DRY_RUN);

            try {
                fileAssociationService.fixFileAssociations();
            } catch (ValidationException) {
                return 1;
            } catch (Exception e) when (!(e is OutOfMemoryException)) {
                LOGGER.Error(e);
                return 2;
            }

            return 0;
        }

        private static void initLogging(LogLevel minLogLevel) {
            var loggingConfiguration = new LoggingConfiguration();
            var consoleTarget        = new ConsoleTarget { Layout = "${level:uppercase=true:truncate=1}|${message}" };

            loggingConfiguration.AddRule(minLogLevel, LogLevel.Fatal, consoleTarget);

            LogManager.Configuration = loggingConfiguration;
        }

    }

}