using System;
using System.Collections;
using System.Collections.Generic;

namespace OCP
{
/**
 * Interface ILogger
 * @package OCP
 * @since 7.0.0
 *
 * This logger interface follows the design guidelines of PSR-3
 * https://github.com/php-fig/fig-standards/blob/master/accepted/PSR-3-logger-interface.md#3-psrlogloggerinterface
 */
public interface ILogger {
//	/**
//	 * @since 14.0.0
//	 */
//	const DEBUG=0;
//	/**
//	 * @since 14.0.0
//	 */
//	const INFO=1;
//	/**
//	 * @since 14.0.0
//	 */
//	const WARN=2;
//	/**
//	 * @since 14.0.0
//	 */
//	const ERROR=3;
//	/**
//	 * @since 14.0.0
//	 */
//	const FATAL=4;

	/**
	 * System is unusable.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void emergency(string message, Array context);

	/**
	 * Action must be taken immediately.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void alert(string message, List<string> context);

	/**
	 * Critical conditions.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void critical(string message, Array context);

	/**
	 * Runtime errors that do not require immediate action but should typically
	 * be logged and monitored.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void error(string message, Array context);

	/**
	 * Exceptional occurrences that are not errors.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void warning(string message, Array context);

	/**
	 * Normal but significant events.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void notice(string message, Array context);

	/**
	 * Interesting events.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void info(string message, Array context);

	/**
	 * Detailed debug information.
	 *
	 * @param string $message
	 * @param array $context
	 * @return null
	 * @since 7.0.0
	 */
	void debug(string message, Array context);

	/**
	 * Logs with an arbitrary level.
	 *
	 * @param int $level
	 * @param string $message
	 * @param array $context
	 * @return mixed
	 * @since 7.0.0
	 */
	void log(int level, string message, Array context);

	/**
	 * Logs an exception very detailed
	 * An additional message can we written to the log by adding it to the
	 * context.
	 *
	 * <code>
	 * $logger->logException($ex, [
	 *     'message' => 'Exception during background job execution'
	 * ]);
	 * </code>
	 *
	 * @param \Exception|\Throwable $exception
	 * @param array $context
	 * @return void
	 * @since 8.2.0
	 */
	void logException(Exception exception, Dictionary<string,object> context);
}
}